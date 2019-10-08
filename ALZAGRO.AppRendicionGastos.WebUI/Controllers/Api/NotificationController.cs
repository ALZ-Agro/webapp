using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers.Api {
    [RoutePrefix("api/notifications")]
    public class NotificationsController : ApiControllerBase {

        private readonly INotificationAppService notificationAppService;

        public NotificationsController(IErrorAppService errorAppService,
                                       INotificationAppService notificationAppService)
            : base(errorAppService) {

            this.notificationAppService = notificationAppService;
        }

        [HttpGet]
        [Route("search")]
        public HttpResponseMessage Search(HttpRequestMessage request, [FromUri] NotificationListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var notificationsDto = this.notificationAppService.Search(criteria);

                response = request.CreateResponse(HttpStatusCode.OK, notificationsDto);
                return response;
            });
        }

        [HttpGet]
        [Route("getUnreadNumber")]
        public HttpResponseMessage GetUnreadNumber(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var notificationCount = this.notificationAppService.GetUnreadNumber();

                response = request.CreateResponse(HttpStatusCode.OK, notificationCount);
                return response;
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;

                var notificationDto = this.notificationAppService.GetById(id);
                response = request.CreateResponse(HttpStatusCode.Created, notificationDto);

                return response;
            });
        }

        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, NotificationDto notificationDto) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;

                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {
                    this.notificationAppService.Save(notificationDto);
                    response = request.CreateResponse(HttpStatusCode.Created, notificationDto);
                }

                return response;
            });
        }
        [HttpGet]
        [Route("update")]
        public HttpResponseMessage UpdateSettings(HttpRequestMessage request, [FromUri]NotificationUpdateCriteria criteria) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;

                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {

                    this.notificationAppService.UpdateSettings(criteria);
                    response = request.CreateResponse(HttpStatusCode.Created, criteria);
                }

                return response;
            });
        }

        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, Int64 id) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var notification = this.notificationAppService.GetById(id);
                if (notification.User.Id == (int)this.CurrentUserId) {
                    this.notificationAppService.DeleteById(id);
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.Unauthorized, "No está autorizado para borrar esta notificación");
                }


                return response;
            });
        }

    }
}