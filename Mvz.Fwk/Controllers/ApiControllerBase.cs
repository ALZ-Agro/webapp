using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.Fwk.Controllers {

    [Authorize]
    public class ApiControllerBase : ApiController {


        protected ValidationResult ValidationResult = new ValidationResult();

        private IErrorAppService errorAppService;
        public ApiControllerBase(IErrorAppService errorAppService) {
            this.errorAppService = errorAppService;
        }


        //TODO: Pendiente hasta que implementemos Auth2.0
        public Int64? CurrentUserId {
            get {

                var customPrincipal = HttpContext.Current.User as CustomPrincipal;
                if (customPrincipal != null) {
                    return customPrincipal.UserId;
                }
                else {
                    return null;
                }
            }
        }

        protected HttpResponseMessage CreateModelValidationErrorResponse(HttpRequestMessage request) {
            return request.CreateResponse(HttpStatusCode.BadRequest,
                       ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                             .Select(m => m.ErrorMessage).ToArray());
        }


        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request,
                                                         Func<HttpResponseMessage> function) {
            HttpResponseMessage response = null;

            try {
                response = function.Invoke();
            }
            catch (Exception ex) {
                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        private void LogError(Exception ex) {
            try {
                Error _error = new Error() {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    //TODO: Este es uno de los dos lugares donde por ahora usamo DateTime.Now
                    //      En caso de encontrar otro deberíamos reemplazarlo por ITimeService.
                    DateCreated = DateTime.Now
                };

                errorAppService.Log(_error);
            }
            catch { }
        }

        protected void AddValidationError(String message) {

            ValidationFailure validationFailure = new ValidationFailure("", message);
            this.ValidationResult.Errors.Add(validationFailure);
        }

        protected void AddValidationError(String propertyName, String message) {

            ValidationFailure validationFailure = new ValidationFailure(propertyName, message);
            this.ValidationResult.Errors.Add(validationFailure);
        }
    }
}