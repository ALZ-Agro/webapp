using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using ALZAGRO.AppRendicionGastos.Application.Dtos;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/payments")]
    public class PaymentsController : ApiControllerBase {

        private readonly IPaymentAppService paymentAppService;

        public PaymentsController(
                                 IPaymentAppService paymentAppService,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.paymentAppService = paymentAppService;
        }


        [HttpGet]
        [Route("ofCurrentUser")]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.paymentAppService.GetUserPaymentModes();
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }
        [HttpGet]
        [Route("of/{id}")]
        public HttpResponseMessage GetPaymentsOf(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.paymentAppService.GetUserPaymentModes(id);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpGet]
        [Route("search")]
        public HttpResponseMessage GetPaymentsOf(HttpRequestMessage request, [FromUri] ListViewCriteriaBase criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.paymentAppService.Search(criteria);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.paymentAppService.GetById(id);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        public HttpResponseMessage Post(HttpRequestMessage request, PaymentDto dto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.paymentAppService.Save(dto);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                this.paymentAppService.DeleteById(id);
                response = request.CreateResponse(HttpStatusCode.OK);

                return response;
            });
        }
    }
}