using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.Mappings;
using ALZAGRO.AppRendicionGastos.Application.Services;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using AutoMapper;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers.Api {

    [RoutePrefix("api/importData")]
    public class ImportDataController : ApiControllerBase {

        private readonly IProviderAppService providerAppService;
        private readonly IDataService dataService;
        private readonly IEntityBaseRepository<LegalCondition> legalConditionRepository;

        public ImportDataController(
        IEntityBaseRepository<LegalCondition> legalConditionRepository,
        IDataService dataService,
        IProviderAppService providerAppService,
            IErrorAppService errorAppService)
            : base(errorAppService) {
            this.dataService = dataService;
            this.legalConditionRepository = legalConditionRepository;
            this.providerAppService = providerAppService;
        }

        [HttpPost]
        [Route("providers")]
        public HttpResponseMessage ExportProviders(HttpRequestMessage request, ProviderListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                //var providerList = this.providerAppService.ExportData(criteria);
                var rejected = new List<ImportProviderDto>();
                var results = this.dataService.ReadCsv<ImportProviderDto>(criteria.FileName);
                foreach(var provider in results) {
                    var dto = Mapper.Map<ImportProviderDto, ProviderDto>(provider);
                    dto.UserId = (int)this.CurrentUserId;
                    var entityLegalCondition = this.legalConditionRepository.GetAll().
                                                                             Where(x => x.Description == provider.LegalCondition).
                                                                             FirstOrDefault();
                    if(entityLegalCondition != null) {
                        dto.LegalCondition = Mapper.Map<LegalCondition,LegalConditionDto>(entityLegalCondition);
                        this.providerAppService.Save(dto);
                    } else {
                        rejected.Add(provider);
                    }
                }
                string rejectedLocation = "";
                if(rejected.Count() > 0) {
                    rejectedLocation = dataService.SaveCsv<ImportProviderDto>(rejected.AsEnumerable(), "Proveedores_rechazados_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                response = request.CreateResponse(HttpStatusCode.Created, new { success = true, path = rejectedLocation});

                return response;
            });
        }

    }
}
