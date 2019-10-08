using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase {

        private readonly IMembershipService membershipService;
        private readonly IUserAppService userAppService;

        public AccountController(IMembershipService membershipService,
                                 IUserAppService userAppService,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.membershipService = membershipService;
            this.userAppService = userAppService;
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginDto user) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                if (ModelState.IsValid) {

                    if (String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.Email)) {
                        response = request.CreateResponse(HttpStatusCode.OK, new {
                            success = false,
                            message = "Los campos email y contraseña son obligatorios."
                        });
                    }

                    MembershipContext _userContext = membershipService.ValidateUser(user.Email, user.Password);
                    
                    if (_userContext.User != null) {
                        var entity = this.userAppService.GetUser(_userContext.User.Id);
                        if (entity.IsLocked) {
                            response = request.CreateResponse(HttpStatusCode.OK, new {
                                success = false,
                                message = "No posees permisos para acceder."
                            });
                            } else {
                            entity.LastActivityDateTime = DateTime.Now;
                            response = request.CreateResponse(HttpStatusCode.OK, new {
                                userId = entity.Id,
                                email = entity.Email,
                                success = true,
                                role = entity.Role,
                                firstName = entity.FirstName,
                                lastName = entity.LastName,
                                enabledCompanies = entity.UserCompanyGroups,
                                showNotifications = entity.ShowNotifications
                            });
                        }
                    }
                    else {
                        response = request.CreateResponse(HttpStatusCode.OK, new {
                            success = false,
                            message = "El email o la contraseña ingresada son incorrectos."
                        });
                    }
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }

                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot")]
        public HttpResponseMessage Forgot(HttpRequestMessage request, ForgotDto forgotDto) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                var userDto = this.userAppService.GetByEmail(forgotDto.Email);

                if (userDto != null) {
                    this.userAppService.ResetPassword(userDto);
                    response = request.CreateResponse(HttpStatusCode.Created, userDto);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        "La dirección de correo que ingresaste no corresponde a ninguna cuenta existente.");
                }

                return response;
            });
        }
    }
}