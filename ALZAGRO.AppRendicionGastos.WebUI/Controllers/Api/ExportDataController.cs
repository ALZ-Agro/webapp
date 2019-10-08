using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.ExtensionMethods;
using ALZAGRO.AppRendicionGastos.Application.Mappings;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.Reports.Data.Entities;
using ALZAGRO.Reports.Data.Interfaces;
using AutoMapper;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using static ALZAGRO.Reports.Data.DataEquivalences;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers.Api
{

    [RoutePrefix("api/exportData")]
    public class ExportDataController : ApiControllerBase
    {

        private readonly IExpenseAppService expenseAppService;
        private readonly IProviderAppService providerAppService;
        private readonly IEntityBaseRepository<Expense> expenseRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IReportsUnnitOfWork reportsUnitOfWork;
        private readonly IReportRepository<ReceiptHeader> receiptHeaderContext;
        private readonly IReportRepository<ReceiptItem> receiptItemContext;
        private readonly IReportRepository<H_Concepts> haberConceptsRepository;
        private readonly IReportRepository<Distribution> distributionContext;
        private readonly IReportRepository<D_Concepts> debeConceptsRepository;
        private readonly IReportRepository<TaxesDetails> taxesDetailsRepository;
        private readonly IReportRepository<DistDimTesoH> distDimTesoHRepository;

        public ExportDataController(IExpenseAppService expenseAppService,
         IUnitOfWork unitOfWork,
         IReportsUnnitOfWork reportsUnitOfWork,
        IReportRepository<ReceiptHeader> receiptHeaderContext,
        IReportRepository<ReceiptItem> receiptItemContext,
        IReportRepository<H_Concepts> haberConceptsRepository,
        IReportRepository<Distribution> distributionContext,
        IReportRepository<D_Concepts> debeConceptsRepository,
        IReportRepository<TaxesDetails> taxesDetailsRepository,
        IReportRepository<DistDimTesoH> distDimTesoHRepository,
        IEntityBaseRepository<Expense> expenseRepository,
        IProviderAppService providerAppService,
            IErrorAppService errorAppService)
            : base(errorAppService)
        {
            this.unitOfWork = unitOfWork;
            this.reportsUnitOfWork = reportsUnitOfWork;
            this.receiptHeaderContext = receiptHeaderContext;
            this.receiptItemContext = receiptItemContext;
            this.haberConceptsRepository = haberConceptsRepository;
            this.distributionContext = distributionContext;
            this.distDimTesoHRepository = distDimTesoHRepository;
            this.debeConceptsRepository = debeConceptsRepository;
            this.taxesDetailsRepository = taxesDetailsRepository;
            this.expenseRepository = expenseRepository;
            this.expenseAppService = expenseAppService;
            this.providerAppService = providerAppService;
        }

        [HttpPost]
        [Route("expenses")]
        public HttpResponseMessage Post(HttpRequestMessage request, ExpenseListViewCriteria criteria)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                // Force params
                // Approved SyncStatusId
                criteria.SyncStatusId = 3;
                // Not exported 
                criteria.Exported = 2;

                // GetAll
                criteria.Size = -1;

                SearchResultViewModel<ExpenseDto> expenseList = this.expenseAppService.Search(criteria);
                SearchResultViewModel<ExportExpenseDto> exportExpenseList = Mapper.Map<SearchResultViewModel<ExpenseDto>, SearchResultViewModel<ExportExpenseDto>>(expenseList);


                string connectionStringsConfigPath = HttpContext.Current.Server.MapPath("~/ConnectionStrings.config");

                XElement doc = XElement.Load($"{connectionStringsConfigPath}");

                string reportsContextConnectionString = doc.Descendants("add")
                                                 .Where(x => (string)x.Attribute("name") == "ReportsContext")
                                                 .Select(x => (string)x.Attribute("connectionString"))
                                                 .FirstOrDefault();
                using (SqlConnection connection =
                    new SqlConnection(reportsContextConnectionString ?? throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.")))
                {
                    const string ProviderAccountNumberQuery = @"SELECT CONVERT(int,PVMPRH_NROCTA) from PVMPRH where @ProveedorId = PVMPRH_NRODOC";

                    List<long> providersNotExported = new List<long>();
                    List<KeyValuePair<long, int>> existingProviders = new List<KeyValuePair<long, int>>();
                    long expensesExported = 0;
                    long expensesNotExported = 0;
                    DateTime CurrentDate = DateTime.Today;

                    foreach (ExportExpenseDto expense in exportExpenseList.Results)
                    {
                        Int32 providerAccountNumber = 0;
                        if (existingProviders.All(x => x.Key != expense.Provider.Cuit))
                        {
                            SqlCommand command = new SqlCommand(ProviderAccountNumberQuery, connection);
                            command.Parameters.AddWithValue("@ProveedorId", expense.Provider.Cuit);
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            try
                            {
                                if (reader.Read())
                                {
                                    providerAccountNumber = reader.GetInt32(0);
                                    if (providerAccountNumber != 0)
                                    {
                                        existingProviders.Insert(0, new KeyValuePair<long, int>(expense.Provider.Cuit, providerAccountNumber));
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            finally
                            {
                                reader.Close();
                                command.Dispose();
                                connection.Close();
                            }
                        }
                        else
                        {
                            providerAccountNumber = existingProviders.Where(x => x.Key == expense.Provider.Cuit).Select(x => x.Value).FirstOrDefault();
                        }

                        if (providerAccountNumber != 0)
                        {
                            bool SIN_IVA = expense.Aliquot.Description == "SIN IVA";
                            string userCompanyGroupForExpense = expense.User.UserCompanyGroups.Where(x => x.Company.Id == expense.CompanyId).
                                                                                            Select(x => x.UserGroup.Code + x.Company.Id).
                                                                                            FirstOrDefault();
                            var stringifiedproviderAccountNumber = providerAccountNumber.ToString();
                            ReceiptHeader header = new ReceiptHeader()
                            {
                                SAR_CORMVH_IDENTI = expense.Id.ToString(),
                                SAR_CORMVH_CIRCOM = SIN_IVA ? CIRCOM.SIN_IVA : CIRCOM.IVA,
                                SAR_CORMVH_CIRAPL = SIN_IVA ? CIRCOM.SIN_IVA : CIRCOM.IVA,
                                SAR_CORMVH_CODEMP = "AL0" + expense.CompanyId,
                                SAR_CORMVH_CODORI = expense.Receipt,
                                SAR_CORMVH_CMPRAD = "N/A",
                                SAR_CORMVH_STATUS = 'N',
                                SAR_CORMVH_FCHMOV = CurrentDate,
                                SAR_CORMVH_NROCTA = stringifiedproviderAccountNumber.PadLeft(6)
                            };

                            receiptHeaderContext.Add(header);

                            var hasNotTaxesConcepts = Math.Abs(double.Parse(expense.NotTaxedConcepts)) > 0;

                            ReceiptItem itemDefault = new ReceiptItem()
                            {
                                SAR_CORMVI_IDENTI = expense.Id.ToString(),
                                SAR_CORMVI_TIPORI = expense.Category.Code,
                                SAR_CORMVI_CANTID = 1,
                                SAR_CORMVI_ARTORI = expense.CompanyId + expense.Category.Code + userCompanyGroupForExpense + "G",
                                SAR_CORMVI_NROITM = 1,
                                SAR_CORMVI_PRECIO = SIN_IVA ? expense.Total : expense.NetValue,
                                SAR_CORMVI_NROAPL = null,
                                SAR_CORMVI_ITMAPL = null
                            };

                            receiptItemContext.Add(itemDefault);

                            CreateDistribution(expense.Id, itemDefault.SAR_CORMVI_NROITM, expense.CompanyId, expense.User.Id_Erp);

                            D_Concepts Debe_concepts_default = new D_Concepts()
                            {
                                SAR_CORMVI08_IDENTI = expense.Id,
                                SAR_CORMVI08_IMPORT = expense.Total
                            };

                            debeConceptsRepository.Add(Debe_concepts_default);

                            H_Concepts Haber_concepts_default = new H_Concepts()
                            {
                                SAR_CORMVI09_IDENTI = expense.Id,
                                SAR_CORMVI09_CODCPT = expense.CompanyId + (expense.Payment.Description == "Efectivo" ? "E" : "T"),
                                SAR_CORMVI09_IMPORT = expense.Total
                            };

                            haberConceptsRepository.Add(Haber_concepts_default);

                            if (hasNotTaxesConcepts)
                            {
                                ReceiptItem secondLine = itemDefault.Clone();
                                secondLine.SAR_CORMVI_NROITM = 2;
                                secondLine.SAR_CORMVI_PRECIO = expense.NotTaxedConcepts;
                                secondLine.SAR_CORMVI_ARTORI = secondLine.SAR_CORMVI_ARTORI.Remove(secondLine.SAR_CORMVI_ARTORI.Length - 1, 1) + "N";
                                receiptItemContext.Add(secondLine);
                                CreateDistribution(expense.Id, secondLine.SAR_CORMVI_NROITM, expense.CompanyId, expense.User.Id_Erp);
                            }

                            string aliquotValue = expense.Aliquot.Value.ToString(CultureInfo.InvariantCulture);
                            
                            if (!SIN_IVA)
                            {
                                TaxesDetails taxDetails = new TaxesDetails()
                                {
                                    SAR_CORMVI07_IDENTI = expense.Id.ToString(),
                                    SAR_CORMVI07_CODCPT = GetCODPT(aliquotValue),
                                    SAR_CORMVI07_INGRES = expense.NetValue,
                                    SAR_CORMVI07_IMPGRA = expense.NetValue,
                                    SAR_CORMVI07_PORCEN = aliquotValue,
                                    SAR_CORMVI07_NROITM = 1
                                };

                                taxesDetailsRepository.Add(taxDetails);
                            }


                            DistDimTesoH distDimTesoH = new DistDimTesoH()
                            {
                                SAR_CJRMVD10_IDENTI = expense.Id,
                                SAR_CJRMVD10_CODDIS = $"AL0{expense.CompanyId.ToString()}|{expense.User.Id_Erp}"
                            };

                            distDimTesoHRepository.Add(distDimTesoH);

                            this.reportsUnitOfWork.Commit();

                            expensesExported++;
                        }
                        else
                        {
                            expensesNotExported++;
                            if (!providersNotExported.Contains(expense.Provider.Cuit))
                            {
                                providersNotExported.Add(expense.Provider.Cuit);
                            }
                        }
                    }
                    foreach (var expense in expenseList.Results)
                    {
                        if (!providersNotExported.Contains(expense.Provider.Cuit))
                        {
                            expense.Exported = true;
                            expense.ExportedDateTime = DateTime.Now;
                            this.expenseRepository.Edit(Mapper.Map<ExpenseDto, Expense>(expense));
                            this.unitOfWork.Commit();
                        }
                    }

                    int notExportedProviders = providersNotExported.Count();
                    string providersPath = null;

                    if (notExportedProviders > 0)
                    {
                        ProviderListViewCriteria providersCriteria = new ProviderListViewCriteria()
                        {
                            Size = -1,
                            FileName = "Proveedores_inexistentes_exportacion_gastos"
                        };
                        SearchResultViewModel<ProviderDto> providersList = this.providerAppService.ExportData(providersCriteria, null, providersNotExported);
                        providersPath = this.SaveCsv<ProviderDto, ProviderListViewCriteria>(providersList,
                            providersCriteria,
                            providersCriteria.FileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                            false, null);
                    }
                    response = request.CreateResponse(HttpStatusCode.Created, new
                    {
                        success = true,
                        exported = expensesExported,
                        not_exported = expensesNotExported,
                        inexistent_providers = notExportedProviders,
                        error_providers_path = providersPath
                    });
                }
                return response;
            });
        }

        [HttpPost]
        [Route("report/providers")]
        public HttpResponseMessage GenerateProvidersReport(HttpRequestMessage request, ProviderListViewCriteria criteria)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                SearchResultViewModel<ProviderDto> expenseList = (SearchResultViewModel<ProviderDto>)this.providerAppService.Search(criteria);
                var exportExpenseList = new SearchResultViewModel<ProviderReportDto>()
                {
                    TotalItems = expenseList.TotalItems,
                    Results = Mapper.Map<IEnumerable<ProviderDto>, IEnumerable<ProviderReportDto>>(expenseList.Results)
                };

                criteria.FileName = "Reporte_Proveedores";

                var fileUrl = this.SaveCsv<ProviderReportDto, ProviderListViewCriteria>(exportExpenseList,
                                                                                      criteria,
                                                                                      criteria.FileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                                                                                      true,
                                                                                      null);


                response = request.CreateResponse(HttpStatusCode.Created, new { success = true, url = fileUrl });

                return response;
            });
        }

        [HttpPost]
        [Route("report/expenses")]
        public HttpResponseMessage GenerateExpensesReport(HttpRequestMessage request, ExpenseListViewCriteria criteria)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                SearchResultViewModel<ExpenseReportDto> tempExpenseList = this.expenseAppService.GetReport(criteria);

                criteria.FileName = "Reporte_Listado_gastos";

                IEnumerable<ExpenseReportDto> oneLineExpenses = tempExpenseList.Results.Where(x => x.PaymentType == "SIN IVA").Select(x =>
                {
                    x.ItemNumber = "1";
                    x.InterfaceValue = x.Total;
                    x.TaxType = "Gravado";
                    return x;
                });
                IEnumerable<ExpenseReportDto> twoLineExpenses = tempExpenseList.Results.Where(x => x.PaymentType != "SIN IVA");
                List<ExpenseReportDto> resolvedTwoLineExpenses = new List<ExpenseReportDto>();

                foreach (ExpenseReportDto expense in twoLineExpenses)
                {
                    bool parseable = System.Double.TryParse(expense.NotTaxedConcepts, out double notTaxedConcepts);
                    if (parseable && notTaxedConcepts > 0)
                    {
                        ExpenseReportDto firstLine = expense.Clone();
                        firstLine.ItemNumber = "1";
                        firstLine.InterfaceValue = firstLine.NetValue;
                        firstLine.TaxType = "Gravado";
                        firstLine.NotTaxedConcepts = "0";
                        ExpenseReportDto secondLine = expense.Clone();
                        secondLine.ItemNumber = "2";
                        secondLine.InterfaceValue = secondLine.NotTaxedConcepts;
                        secondLine.TaxType = "No Gravado";
                        secondLine.NetValue = "0";
                        resolvedTwoLineExpenses.Add(firstLine);
                        resolvedTwoLineExpenses.Add(secondLine);
                    }
                    else
                    {
                        expense.ItemNumber = "1";
                        expense.InterfaceValue = expense.Total;
                        expense.TaxType = "Gravado";
                        resolvedTwoLineExpenses.Add(expense);
                    }

                }

                IEnumerable<ExpenseReportDto> expenseList = oneLineExpenses.Concat(resolvedTwoLineExpenses);

                SearchResultViewModel<ExpenseReportDto> SearchResultExpenseModel = new SearchResultViewModel<ExpenseReportDto>()
                {
                    Results = expenseList,
                    TotalItems = expenseList.Count()
                };

                var fileUrl = this.SaveCsv<ExpenseReportDto, ExpenseListViewCriteria>(SearchResultExpenseModel,
                                                                                      criteria,
                                                                                      criteria.FileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                                                                                      true,
                                                                                      null);
                response = request.CreateResponse(HttpStatusCode.Created, new { success = true, url = fileUrl });

                return response;
            });
        }

        [HttpPost]
        [Route("report/statusChange")]
        public HttpResponseMessage GenerateStatusChangeReport(HttpRequestMessage request, ExpenseListViewCriteria criteria)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                SearchResultViewModel<ExpenseStatusesLogDto> statusChangeDto = this.expenseAppService.GetStatusLogList(criteria);
                criteria.FileName = "Reporte_cambios_de_estado";
                var fileUrl = this.SaveCsv<ExpenseStatusesLogDto, StatusChangeListCriteria>(statusChangeDto,
                                                                                            Mapper.Map<ExpenseListViewCriteria, StatusChangeListCriteria>(criteria),
                                                                                            criteria.FileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                                                                                            true,
                                                                                            null);
                response = request.CreateResponse(HttpStatusCode.Created, new { success = true, url = fileUrl });

                return response;
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("report/test")]
        public HttpResponseMessage TestPRODDB(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                var receipt = this.receiptItemContext.GetAll().Take(2).ToList();
                response = request.CreateResponse(HttpStatusCode.Created, new
                {
                    success = true,
                    url = receipt
                });

                return response;
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("restartTest")]
        public HttpResponseMessage ExportProviders(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                var expenseIdListAsString = this.receiptHeaderContext.GetAll()
                                                            .Where(x => x.SAR_CORMVH_IDENTI != "1")
                                                            .Select(y => y.SAR_CORMVH_IDENTI)
                                                            .ToList();
                var expenseIdList = expenseIdListAsString.ConvertAll(long.Parse);

                var expenses = expenseRepository.GetAll().Where(x => expenseIdList.Contains(x.Id)).ToList();
                foreach (var expense in expenses)
                {
                    expense.Exported = false;
                    expense.ExportedDateTime = null;
                    expenseRepository.Edit(expense);
                }

                unitOfWork.Commit();

                //var headers = receiptHeaderContext.GetAll().Where(x => expenseIdListAsString.Contains(x.SAR_CORMVH_IDENTI)).ToList();
                //foreach (var header in headers) {
                //    receiptHeaderContext.Remove(header);
                //}

                //var receiptItems = receiptItemContext.GetAll().Where(x => expenseIdListAsString.Contains(x.SAR_CORMVI_IDENTI)).ToList();
                //foreach (var item in receiptItems) {
                //    receiptItemContext.Remove(item);
                //}

                //var haber_concepts = haberConceptsRepository.GetAll().Where(x => expenseIdList.Contains(x.SAR_CORMVI09_IDENTI)).ToList();

                //foreach (var haber_concept in haber_concepts) {
                //    haberConceptsRepository.Remove(haber_concept);
                //}

                //var debe_concepts = debeConceptsRepository.GetAll().Where(x => expenseIdList.Contains(x.SAR_CORMVI08_IDENTI)).ToList();

                //foreach (var debe_concept in debe_concepts) {
                //    debeConceptsRepository.Remove(debe_concept);
                //}

                //var taxesDetails = taxesDetailsRepository.GetAll().Where(x => expenseIdListAsString.Contains(x.SAR_CORMVI07_IDENTI)).ToList();

                //foreach (var taxDetail in taxesDetails) {
                //    taxesDetailsRepository.Remove(taxDetail);
                //}

                //var distDimTesoHs = distDimTesoHRepository.GetAll().Where(x => expenseIdList.Contains(x.SAR_CJRMVD10_IDENTI)).ToList();

                //foreach (var distDimTesoH in distDimTesoHs) {
                //    distDimTesoHRepository.Remove(distDimTesoH);
                //}

                //this.reportsUnitOfWork.Commit();



                response = request.CreateResponse(HttpStatusCode.Created, new { success = true });

                return response;
            });
        }


        [HttpPost]
        [Route("providers")]
        public HttpResponseMessage ExportProviders(HttpRequestMessage request, ProviderListViewCriteria criteria)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                var providerList = this.providerAppService.ExportData(criteria, false, null);

                var fileUrl = this.SaveCsv<ProviderDto, ProviderListViewCriteria>(providerList,
                    criteria,
                    criteria.FileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                    false, null);
                foreach (var provider in providerList.Results)
                {
                    provider.SyncedWithERP = true;
                    this.providerAppService.Save(provider);
                }

                response = request.CreateResponse(HttpStatusCode.Created, new { success = true, url = fileUrl });

                return response;
            });
        }
        public String SaveCsv<T, E>(SearchResultViewModel<T> data, E criteria, String FileName, bool ShowFilterParams, DateTime? dateOfExportation)
        {
            var fileUrl = "/Resources/Export/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (File.Exists(path))
            {
                throw new Exception("Ya existe un archivo con este nombre, por favor especifique otro.");
            }
            using (TextWriter sw = new StreamWriter(path))
            {
                var csv = new CsvWriter(sw);
                //Register mappers
                long pageData = (int)Math.Ceiling(((data.TotalItems / (criteria as ListViewCriteriaBase).Size) + 0.5));
                //csv.Configuration.RegisterClassMap(new ExpenseMap(dateOfExportation != null ? (DateTime)dateOfExportation : DateTime.Now));
                csv.Configuration.RegisterClassMap(new ProviderListViewCriteriaMap(pageData));
                csv.Configuration.RegisterClassMap(new StatusChangeListViewCriteriaMap(pageData));
                csv.Configuration.RegisterClassMap(new ExpenseListViewCriteriaMap(pageData));
                csv.Configuration.RegisterClassMap<ExpenseStatusLogMap>();
                csv.Configuration.RegisterClassMap<ProviderMap>();
                csv.Configuration.RegisterClassMap<NewProviderReportMap>();
                csv.Configuration.RegisterClassMap<ExpenseReportDtoMap>();

                //Filtros
                if (ShowFilterParams)
                {
                    csv.WriteComment("Parámetros");
                    csv.NextRecord();

                    csv.WriteHeader<E>();
                    csv.NextRecord();
                    csv.WriteRecord<E>(criteria);

                    csv.NextRecord();
                    csv.NextRecord();
                }
                //Datos
                csv.WriteHeader<T>();
                csv.NextRecord();
                csv.WriteRecords<T>(data.Results);
                csv.Flush();
            }

            return ConfigurationManager.AppSettings["Host"].ToString() + fileUrl;
        }

        public String SaveCsv<T>(IEnumerable<T> data, String FileName)
        {
            var fileUrl = "/Resources/Export/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (File.Exists(path))
            {
                throw new Exception("Ya existe un archivo con este nombre, por favor especifique otro.");
            }
            using (TextWriter sw = new StreamWriter(path))
            {
                var csv = new CsvWriter(sw);
                //Register mappers
                csv.Configuration.RegisterClassMap<ImportProviderMap>();
                //Datos
                csv.WriteHeader<T>();
                csv.NextRecord();
                csv.WriteRecords<T>(data);
                csv.Flush();
            }

            return ConfigurationManager.AppSettings["Host"].ToString() + fileUrl;
        }

        public List<T> ReadCsv<T>(String FileName)
        {
            var fileUrl = "/Resources/Import/" + FileName + ".csv";
            var path = HttpContext.Current.Server.MapPath("~" + fileUrl);
            if (!File.Exists(path))
            {
                throw new Exception("No se encontró el archivo a importar.");
            }
            if (path == null) return new List<T>();
            if (path == "") return new List<T>();



            List<T> results = new List<T>();


            using (TextReader reader = File.OpenText(path))
            {
                try
                {

                    var csv = new CsvReader(reader);

                    csv.Configuration.RegisterClassMap<ImportProviderMap>();

                    results = csv.GetRecords<T>().ToList();

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Data["CsvHelper"]);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            return results;
        }


        /// HELPERS 

        public void CreateDistribution(Int64 ExpenseId, int nroItem, Int64 CompanyId, String UserCode)
        {
            for (var index = 1; index < 3; index++)
            {
                Distribution distribution = new Distribution()
                {
                    SAR_CORMVD01_IDENTI = ExpenseId,
                    SAR_CORMVD01_NROITM = nroItem,
                    SAR_CORMVD01_NROITD = index,
                    SAR_CORMVD01_CODDIM = GetCODDIM(index),
                    SAR_CORMVD01_CODDIS = GetCODDIS(index, CompanyId, UserCode)
                };
                distributionContext.Add(distribution);
            }
        }


    }
}
