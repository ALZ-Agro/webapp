using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.UI;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class ExpenseAppService : EntityBaseAppService<Expense, ExpenseDto>, IExpenseAppService {

        #region Variables
        private readonly ITimeService timeService;
        private readonly IEntityBaseRepository<Image> imagesRepository;
        private readonly IEntityBaseRepository<Config> configRepository;
        private readonly IEntityBaseRepository<Expense> expensesRepository;
        private readonly IEntityBaseRepository<Provider> providerRepository;
        private readonly IEntityBaseRepository<SyncStatus> syncStatusesRepository;
        private readonly IEntityBaseRepository<User> userRepository;
        private readonly INotificationAppService notificationAppService;
        private readonly IEntityBaseRepository<RefusalReason> refusalReasonsRepository;
        private readonly IEntityBaseRepository<ExpenseStatusLog> expenseStatusLogRepository;
        private readonly IEntityBaseRepository<Category> categoryRepository;
        #endregion

        public ExpenseAppService(IEntityBaseRepository<Expense> expensesRepository,
                              IEntityBaseRepository<Error> errorsRepository,
                              IEntityBaseRepository<Config> configRepository,
                         INotificationAppService notificationAppService,
                              IEntityBaseRepository<Provider> providerRepository,
                              IEntityBaseRepository<Category> categoryRepository,
                              IEntityBaseRepository<ExpenseStatusLog> expenseStatusLogRepository,
                              IEntityBaseRepository<User> userRepository,
                              IEntityBaseRepository<RefusalReason> refusalReasonsRepository,
                              IEntityBaseRepository<Image> imagesRepository,
                              IEntityBaseRepository<SyncStatus> syncStatusesRepository,
                              IUnitOfWork unitOfWork,
                              ITimeService timeService) :
            base(errorsRepository, unitOfWork, expensesRepository) {
            this.timeService = timeService;
            this.categoryRepository = categoryRepository;
            this.configRepository = configRepository;
            this.expenseStatusLogRepository = expenseStatusLogRepository;
            this.imagesRepository = imagesRepository;
            this.expensesRepository = expensesRepository;
            this.providerRepository = providerRepository;
            this.refusalReasonsRepository = refusalReasonsRepository;
            this.syncStatusesRepository = syncStatusesRepository;
            this.userRepository = userRepository;
            this.notificationAppService = notificationAppService;
        }

        public List<SearchUserDto> GetExpenseUsersByCompanyId(long companyId) {
            var query = this.entityRepository.GetAllIncluding(x => x.User).
                                              Where(x => x.CompanyId == companyId).
                                              Select(x => x.User).
                                              Distinct().
                                              ToList();
            return Mapper.Map<List<User>, List<SearchUserDto>>(query);
        }

        private IQueryable<Expense> GetExpenseQuery(ExpenseListViewCriteria criteria) {
            var config = this.configRepository.GetAll().OrderByDescending(x => x.UpdatedDateTime).FirstOrDefault();
            var localTimeNow = this.timeService.LocalDateTimeNow;

            var query = this.entityRepository.GetAllIncluding(x => x.Provider,
                                                              x => x.Provider.LegalCondition,
                                                              x => x.Category,
                                                              x => x.Images,
                                                              x => x.User,
                                                              x => x.User.UserCompanyGroups,
                                                              x => x.User.UserCompanyGroups.Select(y => y.Company),
                                                              x => x.User.UserCompanyGroups.Select(y => y.UserGroup),
                                                              x => x.SyncStatus,
                                                              x => x.Logs,
                                                              x => x.Aliquot,
                                                              x => x.Payment,
                                                              x => x.Company).
                                              Where(x => x.Company.Id == criteria.CompanyId);

            if (!criteria.GetForBackEnd) {
                query = query.Where(x => DbFunctions.DiffDays(x.Date, localTimeNow) <= config.SyncDays);
            }

            if (criteria.CategoryId != 0) {
                query = query.Where(x => x.CategoryId == criteria.CategoryId);
            }

            var user = this.userRepository.GetAllIncluding(x => x.Role).Where(x => x.Id == (int)this.CurrentUserId).FirstOrDefault();

            if (user.Role.Description == "Usuario") {
                query = query.Where(x => x.User.Id == (int)this.CurrentUserId);
            }
            else {
                if (criteria.UserId != 0) {
                    query = query.Where(x => x.User.Id == criteria.UserId);
                }
            }

            if (criteria.StartDate != null) {
                query = query.Where(x => DbFunctions.DiffDays(criteria.StartDate, x.Date) >= 0);
            }

            if (criteria.EndDate != null) {
                query = query.Where(x => DbFunctions.DiffDays(criteria.EndDate, x.Date) <= 0);
            }


            if (criteria.ProviderId != 0) {
                query = query.Where(x => x.Provider.Id == criteria.ProviderId);
            }

            if (criteria.SyncStatusId != 0) {
                query = query.Where(x => x.SyncStatus.Id == criteria.SyncStatusId);
            }

            if (criteria.PaymentId != 0) {
                query = query.Where(x => x.Payment.Id == criteria.PaymentId);
            }

            if (criteria.Exported != 0) {
                if (criteria.Exported == 1) {
                    query = query.Where(x => x.Exported.Equals(true));
                }
                if (criteria.Exported == 2) {
                    query = query.Where(x => x.Exported.Equals(false));
                }
            }

            if (!string.IsNullOrEmpty(criteria.PartialDescription)) {
                query = this.MatchInFields<Expense>(query, criteria.PartialDescription, true, c => new[] {
                    c.Provider.LegalName.ToLower(),
                    c.Total.ToString(),
                    c.Provider.Cuit.ToString()
                });
            }

            return query;
        }

        public SearchResultViewModel<ExpenseDto> Search(ExpenseListViewCriteria criteria) {
            return this.CreateResult<Expense, ExpenseDto>(this.GetExpenseQuery(criteria), criteria, "Id desc");
        }  

        public SearchResultViewModel<ExpenseReportDto> GetReport(ExpenseListViewCriteria criteria) {
            return this.CreateResult<Expense, ExpenseReportDto>(this.GetExpenseQuery(criteria), criteria, "Id desc");
        }

        public List<ExpenseDto> GenerateExpensesToExport(List<long> expensesIds) {
            var query = this.entityRepository.GetAllIncluding(x => x.Provider,
                                                              x => x.Category,
                                                              x => x.Images,
                                                              x => x.User,
                                                              x => x.SyncStatus,
                                                              x => x.Logs,
                                                              x => x.Provider.LegalCondition,
                                                              x => x.Aliquot,
                                                              x => x.Payment,
                                                              x => x.Company).
                                                Where(x => expensesIds.Contains(x.Id)).
                                                OrderBy(x => x.Date).ToList();


            return Mapper.Map<List<Expense>, List<ExpenseDto>>(query);
        }

        public ChangeExpenseStatusResult ChangeStatus(ChangeExpenseStatusDto entity) {
            var result = new ChangeExpenseStatusResult();
            var currentUser = this.userRepository.GetAllIncluding(x => x.Role).
                                                  Where(x => x.Id == (int)this.CurrentUserId).
                                                  FirstOrDefault();
            var expense = this.entityRepository.GetAllIncluding(x => x.SyncStatus,
                                                                    x => x.Provider,
                                                                    x => x.Category,
                                                                    x => x.Payment).
                                                    Where(x => x.Id == entity.Id).
                                                    FirstOrDefault();
            
                if (expense != null) {

                /// Previus SyncStatus == "Editar"
                var newStatusLogEntry = new ExpenseStatusLog() {
                    Id = 0,
                    Change = expense.SyncStatus.Description + " 🡆 " + entity.SyncStatus.Description,
                    ExpenseId = entity.Id,
                    Notes = "N/A",
                    UserId = (int)this.CurrentUserId,
                    ReasonOfChange = "Solucionar errores en datos."
                };
                if (currentUser.Role.Description != "Usuario") {
                    if (string.IsNullOrEmpty(entity.ChangeStatusNote)) {
                        newStatusLogEntry.Notes = "N/A";
                    } else {
                        newStatusLogEntry.Notes = entity.ChangeStatusNote;
                    }

                    if (string.IsNullOrEmpty(entity.ChangeStatusReason)) {
                        newStatusLogEntry.ReasonOfChange = "N/A";
                    }
                    else {
                        newStatusLogEntry.ReasonOfChange = entity.ChangeStatusReason;
                    }
                }

                this.expenseStatusLogRepository.Add(newStatusLogEntry);
                this.unitOfWork.Commit();

                expense.SyncStatus = Mapper.Map<SyncStatusDto, SyncStatus>(entity.SyncStatus);
                expense.SyncStatusId = entity.SyncStatus.Id;
                expense.UpdatedBy = (int)this.CurrentUserId;
                if (entity.SyncStatus != null && entity.SyncStatus.Description == "Rechazado") {
                    this.notificationAppService.CreateNewForExpense(expense, newStatusLogEntry);
                }
                this.entityRepository.Edit(expense);
                this.unitOfWork.Commit();

                result.Success = true;
                result.Message = "Éxito";
            }
            else {
                result.Success = false;
                result.Message = "No se encontró el gasto que se intenta editar.";
            }
            return result;
        }

        public ChangeExpenseStatusResult SetEditingState(long Id) {
            var result = new ChangeExpenseStatusResult();
            try {
                var expense = this.expensesRepository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
                var editStatus = this.syncStatusesRepository.GetAll().Where(x => x.Description == "Editando").FirstOrDefault();
                expense.SyncStatusId = editStatus.Id;
                expense.UpdatedBy = (int)this.CurrentUserId;
                this.unitOfWork.Commit();
                result.Success = true;
                result.Message = "Edición permitida.";
            }
            catch (Exception e) {
                result.Success = false;
                result.Message = e.Message;
            }
            return result;
        }

        public SearchResultViewModel<ExpenseStatusesLogDto> GetStatusLogList(ExpenseListViewCriteria criteria) {
            var query = this.expenseStatusLogRepository.GetAllIncluding(x => x.User, 
                                                                        x => x.Expense, 
                                                                        x => x.Expense.User);
            if(!string.IsNullOrEmpty(criteria.PartialDescription)) {
                query = this.MatchInFields<ExpenseStatusLog>(query, criteria.PartialDescription, true, c => new[] {
                    c.User.FirstName.ToLower(),
                    c.User.LastName.ToLower(),
                    c.Expense.User.FirstName.ToLower(),
                    c.Expense.User.LastName.ToLower(),
                    c.Notes.ToLower(),
                    c.ReasonOfChange.ToLower()
                });
            }
            if (criteria.StartDate != null) {
                query = query.Where(x => DbFunctions.DiffDays(criteria.StartDate, x.UpdatedDateTime) >= 0);
            }

            if (criteria.EndDate != null) {
                query = query.Where(x => DbFunctions.DiffDays(criteria.EndDate, x.UpdatedDateTime) <= 0);
            }

            if(criteria.UserId != 0) {
                query = query.Where(x => x.Expense.UserId == criteria.UserId);
            }
            return this.CreateResult<ExpenseStatusLog, ExpenseStatusesLogDto>(query, criteria, "UpdatedDateTime desc");
        }

        public SearchResultViewModel<ExpenseStatusesLogDto> GetStatusesLogOfExpense(ExpenseListViewCriteria criteria) {
            var query = this.expenseStatusLogRepository.GetAllIncluding(x => x.User).
                                                        Where(x => x.ExpenseId == criteria.ExpenseId);
            return this.CreateResult<ExpenseStatusLog, ExpenseStatusesLogDto>(query, criteria, "UpdatedDateTime desc");
        }

        public override ExpenseDto Save(ExpenseDto expenseDto) {
            //try{
                var expenseUser = this.userRepository.GetAllIncluding(x => x.UserCompanyGroups,
                                                                      x => x.UserCompanyGroups.Select(y => y.Company),
                                                                      x => x.UserCompanyGroups.Select(y => y.UserGroup)).
                                                      Where(x => x.Id == expenseDto.UserId).
                                                      FirstOrDefault();
                /// entidad intermedia entre Compañía-Grupo de la compañía a la que se le agrega el gasto.
                var currentCompanyUserGroup = expenseUser.UserCompanyGroups.Where(x => x.CompanyId == expenseDto.CompanyId).FirstOrDefault();
                if(currentCompanyUserGroup != null) {
                    expenseDto.Group = currentCompanyUserGroup.UserGroup.Description + " " + currentCompanyUserGroup.Company.Name;
                } else {
                    expenseDto.Group = "N/A";
                }
                var provider = this.providerRepository.GetAll().Where(x => x.Cuit == expenseDto.Provider.Cuit).FirstOrDefault();

                if (provider != null) {
                    expenseDto.Provider = Mapper.Map<Provider, ProviderDto>(provider);
                }
                else {
                    var dbProvider = Mapper.Map<ProviderDto, Provider>(expenseDto.Provider);
                    dbProvider.UserId = (int)this.CurrentUserId;
                    this.providerRepository.Add(dbProvider);
                    try {
                        this.unitOfWork.Commit();
                    } catch (Exception ex) {
                        throw new Exception("Catch en metodo save uow1: " + ex.ToString() );
                    }
                expenseDto.Provider.Id = dbProvider.Id;
                }
                if (expenseDto.Id == 0) {
                    var duplicatedSyncStatus = this.syncStatusesRepository.GetAll().
                                                                           Where(x => x.Description == "Duplicado").
                                                                           FirstOrDefault();
                    //var receiptIsEmpty = String.IsNullOrEmpty(expenseDto.Receipt);
                    //var totalAsDouble = StringExtensionMethods.ToDouble(expenseDto.Total);
                    var expensesMatching = this.entityRepository.GetAllIncluding(x => x.Provider,
                                                                                 x => x.Category,
                                                                                 x => x.SyncStatus,
                                                                                 x => x.Company).
                                                                Where(x => x.Provider.Cuit == expenseDto.Provider.Cuit &&
                                                                           x.Receipt != null && 
                                                                           x.Receipt != "" && 
                                                                           x.Receipt == expenseDto.Receipt).
                                                                ToList().
                                                                OrderBy(x => x.UpdatedDateTime);

                    if (expensesMatching.Count() > 0) {
                        expenseDto.SyncStatus = Mapper.Map<SyncStatus, SyncStatusDto>(duplicatedSyncStatus);
                    }
                    expenseDto.UpdatedBy = (int)this.CurrentUserId;
                    var expense = Mapper.Map<ExpenseDto, Expense>(expenseDto);
                    this.entityRepository.Add(expense);
                try {
                    this.unitOfWork.Commit();
                } catch (Exception ex) {
                    throw new Exception("Catch en metodo save uow2:");
                }
                expenseDto.Id = expense.Id;

                    if(expensesMatching.Count() > 0) {
                        var expenseStateLogEntry = new ExpenseStatusLog() {
                            Id = 0,
                            ReasonOfChange = "Carga duplicada.",
                            Notes = "Se detectó que este gasto puede estar duplicado. Será inspeccionado por Administración.",
                            ExpenseId = expenseDto.Id,
                            UserId = (int)this.CurrentUserId,
                            Change = "Nuevo 🡆 Duplicado"
                        };
                        this.expenseStatusLogRepository.Add(expenseStateLogEntry);
                    try {
                        this.unitOfWork.Commit();
                    } catch (Exception ex) {
                        throw new Exception("Catch en metodo save uow3:" );
                    }
                } else {
                        var expenseStateLogEntry = new ExpenseStatusLog() {
                            Id = 0,
                            ReasonOfChange = "N/A",
                            UserId = (int)this.CurrentUserId,
                            Notes = "N/A",
                            ExpenseId = expenseDto.Id,
                            Change = "Nuevo 🡆 " + expenseDto.SyncStatus.Description
                        };
                        this.expenseStatusLogRepository.Add(expenseStateLogEntry);
                    try {
                        this.unitOfWork.Commit();
                    } catch (Exception ex) {
                        throw new Exception("Catch en metodo save uow4:" );
                    }
                }

                    if (expenseDto.Images.Count() > 0) {
                        foreach (var image in expenseDto.Images) {
                            var img = Mapper.Map<ImageDto, Image>(image);
                            img.ExpenseId = expenseDto.Id;
                            this.imagesRepository.Add(img);
                            try {
                                this.unitOfWork.Commit();
                            } catch (Exception ex) {
                                throw new Exception("Catch en metodo save uow5:" + ex.ToString());
                            }
                        }
                    }
                }
                else {
                    var dbExpense = this.entityRepository.GetAllIncluding(x => x.SyncStatus).
                                                          Where(x => x.Id == expenseDto.Id).
                                                          FirstOrDefault();
                    expenseDto.UpdatedBy = (int)this.CurrentUserId;
                    var expense = Mapper.Map<ExpenseDto, Expense>(expenseDto);
                    this.entityRepository.Edit(expense);
                try {
                    this.unitOfWork.Commit();
                } catch (Exception ex) {
                    throw new Exception("Catch en metodo save uow6:" );
                }
                if (dbExpense.SyncStatus.Description != expenseDto.SyncStatus.Description) {
                        var expenseStateLogEntry = new ExpenseStatusLog() {
                            Id = 0,
                            ReasonOfChange = "Corrección de datos.",
                            Notes = "N/A",
                            UserId = (int)this.CurrentUserId,
                            ExpenseId = expenseDto.Id,
                            Change = dbExpense.SyncStatus.Description + " 🡆 " + expenseDto.SyncStatus.Description
                        };
                        this.expenseStatusLogRepository.Add(expenseStateLogEntry);
                    try {
                        this.unitOfWork.Commit();
                    } catch (Exception ex) {
                        throw new Exception("Catch en metodo save uow7:");
                    }
                }
                }

                return expenseDto;
            //}catch(Exception ex){
            //    throw new Exception("Catch en metodo save:" + ex.ToString());
            //}
        }


        public ExpensesListSearchResultViewModel<ExpenseListDto> GetExpensesList(ExpenseListViewCriteria criteria) {
            criteria.GetForBackEnd = true;
            var searchResultQuery = this.GetExpenseQuery(criteria);
            var searchResult = this.CreateResult<Expense, ExpenseDto>(searchResultQuery, criteria, "Id desc");
            var ExpenseList = new ExpensesListSearchResultViewModel<ExpenseListDto>() {
                Results = Mapper.Map<IEnumerable<ExpenseDto>, IEnumerable<ExpenseListDto>>(searchResult.Results),
                TotalItems = searchResult.TotalItems,
                SubTotal = searchResultQuery.Select(x => x.Total).
                                             DefaultIfEmpty(0).
                                             Sum()
            };
            return ExpenseList;
        }


        public ExpenseDto GetSingle(long Id) {
            var query = this.entityRepository.GetAllIncluding(x => x.Provider,
                                                              x => x.Category,
                                                              x => x.Images,
                                                              x => x.User,
                                                              x => x.User.UserCompanyGroups,
                                                              x => x.User.UserCompanyGroups.Select(y => y.Company),
                                                              x => x.User.UserCompanyGroups.Select(y => y.UserGroup),
                                                              x => x.SyncStatus,
                                                              x => x.Logs,
                                                              x => x.Aliquot,
                                                              x => x.Payment,
                                                              x => x.Company).
                                                Where(x => x.Id == Id).
                                                FirstOrDefault();
            return Mapper.Map<Expense, ExpenseDto>(query);
        }

        public AnalyticsDto GetAnalytics(ExpenseListViewCriteria criteria) {
            var currentUser = this.userRepository.GetAllIncluding(x => x.Role).Where(x => x.Id == (int)this.CurrentUserId).FirstOrDefault();

            var query = this.entityRepository.GetAllIncluding(x => x.Provider,
                                                               x => x.User,
                                                               x => x.SyncStatus,
                                                               x => x.Company).
                                               Where(x => x.Company.Id == criteria.CompanyId &&
                                                    DbFunctions.DiffDays(criteria.StartDate, x.Date) >= 0 &&
                                                    DbFunctions.DiffDays(criteria.EndDate, x.Date) <= 0);

            if (currentUser.Role.Description == "Usuario") {
                query = query.Where(x => x.User.Id == (int)this.CurrentUserId);
            }
            if (query.Count() == 0) {
                return new AnalyticsDto();
            }
            var results = query.GroupBy(x => x.User.Id).ToList();

            var analyticsDto = new AnalyticsDto();
            analyticsDto.Vendors = new List<VendorAnalyticsDto>();

            foreach (var userExpenses in results) {
                var user = this.userRepository.GetSingle(userExpenses.Key);
                var vendorAnalytics = new VendorAnalyticsDto {
                    FullName = user.FirstName + " " + user.LastName
                };

                foreach (var expense in userExpenses) {
                    var dbExpense = this.entityRepository.GetAllIncluding(x => x.Category).Where(x => x.Id == expense.Id).FirstOrDefault();
                    switch (dbExpense.Category.Description) {
                        case "Comida":
                            analyticsDto.TotalFood = analyticsDto.TotalFood + expense.Total;
                            vendorAnalytics.TotalFood = vendorAnalytics.TotalFood + expense.Total;
                            break;
                        case "Alojamiento":
                            analyticsDto.TotalHotel = analyticsDto.TotalHotel + expense.Total;
                            vendorAnalytics.TotalHotel = vendorAnalytics.TotalHotel + expense.Total;
                            break;
                        case "Combustible":
                            analyticsDto.TotalFuel = analyticsDto.TotalFuel + expense.Total;
                            vendorAnalytics.TotalFuel = vendorAnalytics.TotalFuel + expense.Total;
                            break;
                        default:
                            analyticsDto.TotalOther = analyticsDto.TotalOther + expense.Total;
                            vendorAnalytics.TotalOther = vendorAnalytics.TotalOther + expense.Total;
                            break;
                    }
                }
                analyticsDto.Vendors.Add(vendorAnalytics);
            }

            return analyticsDto;
        }
    }
}