using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ALZAGRO.AppRendicionGastos.Application.Dtos;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/syncStatus")]
    public class SyncStatusController : ApiControllerBase {

        private readonly IEntityBaseRepository<SyncStatus> syncStatusRepository;

        public SyncStatusController(
                                 IEntityBaseRepository<SyncStatus> syncStatusRepository,
        IErrorAppService errorAppService)
            : base(errorAppService) {
            this.syncStatusRepository = syncStatusRepository;
        }
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<List<SyncStatus>, List<SyncStatusDto>>(this.syncStatusRepository.GetAll().ToList()));

                return response;
            });
        }
    }
}