using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers
{

    [RoutePrefix("api/provider")]
    public class ProviderController : ApiControllerBase {

        private readonly IProviderAppService providerAppService;

        public ProviderController(
                                 IProviderAppService providerAppService,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.providerAppService = providerAppService;
        }


        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.providerAppService.GetAll();
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpGet]
        [Route("parking")]
        public HttpResponseMessage GetDefault(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var def = this.providerAppService.GetAll().
                                                    Where(x => x.SyncedWithERP.Equals(true) &&
                                                                        x.LegalName.Contains("Peajes y Estacionamientos")).
                                                    FirstOrDefault();
                response = request.CreateResponse(HttpStatusCode.OK, def);

                return response;
            });
        }

        [HttpGet]
        [Route("getDefault")]
        public HttpResponseMessage GetParking(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var def = this.providerAppService.GetAll().
                                                    Where(x => x.SyncedWithERP.Equals(true) && 
                                                                        x.LegalName.Contains("Gastos Varios")).
                                                    FirstOrDefault();
                response = request.CreateResponse(HttpStatusCode.OK, def);

                return response;
            });
        }

        [Route("search")]
        [HttpGet]
        public HttpResponseMessage Search(HttpRequestMessage request, [FromUri] ProviderListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                    var items = this.providerAppService.Search(criteria);
                    response = request.CreateResponse(HttpStatusCode.OK, items);
                
                return response;
            });
        }

        [HttpPost]
        public HttpResponseMessage Save(HttpRequestMessage request, ProviderDto providerDto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                if (ModelState.IsValid) {

                    this.providerAppService.Save(providerDto);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, Id = providerDto.Id });
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }

                return response;
            });
        }

        [HttpPost]
        [Route("import")]
        public HttpResponseMessage ImportProviders(HttpRequestMessage request, List<ProviderDto> providers) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var errors = providerAppService.Import(providers);
                response = request.CreateResponse(HttpStatusCode.OK, new { Errors = errors  });

                return response;
            });
        }


        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                this.providerAppService.DeleteById(id);
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true });

                return response;
            });
        }
    }
}