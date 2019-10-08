using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase {

        private readonly IUserAppService userAppService;
        private readonly IEntityBaseRepository<Role> roleRepository;
        private readonly IMembershipService membershipService;
        private readonly IEntityBaseRepository<UserGroup> groupRepository;

        public UsersController(IErrorAppService errorAppService,
                               IUserAppService userAppService,
                         IEntityBaseRepository<UserGroup> groupRepository,
                               IEntityBaseRepository<Role> roleRepository,
                               IMembershipService membershipService)
            : base(errorAppService) {
            this.groupRepository = groupRepository;
            this.roleRepository = roleRepository;
            this.userAppService = userAppService;
            this.membershipService = membershipService;
        }

        [HttpGet]
        [Route("isLocked/{id}")]
        public HttpResponseMessage IsLocked(HttpRequestMessage request, Int32 id) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                var user = this.userAppService.GetById(id);
                bool isLocked = false;
                if(user == null || (user != null && user.IsLocked)) {
                    isLocked = true;
                }
                response = request.CreateResponse(HttpStatusCode.OK, isLocked);

                return response;
            });
        }

        [HttpGet]
        [Route("search")]
        public HttpResponseMessage Search(HttpRequestMessage request, [FromUri] UserListViewCriteria criteria) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                var items = this.userAppService.Search(criteria);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpGet]
        [Route("getAll")]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var usersDto = this.userAppService.GetAll();

                response = request.CreateResponse(HttpStatusCode.OK, usersDto);

                return response;
            });
        }


        [HttpGet]
        [Route("expenseUsers")]
        public HttpResponseMessage GetForExpenseList(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var users = this.userAppService.GetForFilter();
                response = request.CreateResponse(HttpStatusCode.OK,users);
                return response;
            });
        }
        [Route("get/{id}")]
        public HttpResponseMessage Get(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var userDto = this.userAppService.GetUser(id);

                response = request.CreateResponse(HttpStatusCode.OK, userDto);

                return response;
            });
        }

        [HttpGet]
        [Route("roles")]
        public HttpResponseMessage GetRoles(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var currentUser = this.userAppService.GetUser((int)this.CurrentUserId);
                if(currentUser != null && currentUser.Role.Description == "Administrador del sistema") {
                    var roles = Mapper.Map<List<Role>,List<RoleDto>>(this.roleRepository.GetAll().ToList());
                    response = request.CreateResponse(HttpStatusCode.OK, roles);
                } else {
                    response = request.CreateResponse(HttpStatusCode.Forbidden, new {
                        message = "No tenés permiso."
                    });
                }

                return response;
            });
        }

        [HttpGet]
        [Route("groups")]
        public HttpResponseMessage GetUserGroups(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {

                HttpResponseMessage response = null;
                var currentUser = this.userAppService.GetUser((int)this.CurrentUserId);
                if (currentUser != null && currentUser.Role.Description == "Administrador del sistema") {
                    var groups = Mapper.Map<List<UserGroup>, List<UserGroupDto>>(this.groupRepository.GetAll().ToList());
                    response = request.CreateResponse(HttpStatusCode.OK, groups);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.Forbidden, new {
                        message = "No tenés permiso."
                    });
                }

                return response;
            });
        }

        [HttpPost]
        [Route("updateLastActivityDateTime/{id}")]
        public HttpResponseMessage UpdateLastActivityDateTime(HttpRequestMessage request, Int64 id) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {
                    this.userAppService.UpdateLastActivityDateTime(id);
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }

                return response;
            });
        }

     


        [AllowAnonymous]
        public HttpResponseMessage Post(HttpRequestMessage request, UserDto userDto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {
                    var usernameExists = this.membershipService.UserExist(userDto.Id, userDto.Username);
                    var emailExists = this.membershipService.UserEmailExist(userDto.Id, userDto.Email);

                    if (usernameExists || emailExists) {
                        if (usernameExists) {
                            response = request.CreateResponse(HttpStatusCode.BadRequest,
                                        String.Format("El nombre de usuario {0} ya está asignado.",
                                        userDto.Username));
                        }
                        else if (emailExists) {
                            response = request.CreateResponse(HttpStatusCode.BadRequest,
                                        String.Format("El email {0} ya está asignado.",
                                        userDto.Email));
                        }
                    }
                    else {
                        var resultUserDto = this.userAppService.Save(userDto);

                        response = request.CreateResponse(HttpStatusCode.Created, resultUserDto);
                    }

                }
                return response;
            });
        }

        [HttpPost]
        [Route("resetPassword")]
        public HttpResponseMessage ResetPassword(HttpRequestMessage request, UserDto userDto) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                this.userAppService.ResetPassword(userDto);
                response = request.CreateResponse(HttpStatusCode.Created, userDto);

                return response;
            });
        }

        [HttpPost]
        [Route("changePassword")]
        public HttpResponseMessage ChangePassword(HttpRequestMessage request, ChangePasswordDto changePasswordDto) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {
                    this.userAppService.ChangePassword(changePasswordDto);
                    response = request.CreateResponse(HttpStatusCode.OK, changePasswordDto);
                }

                return response;
            });
        }
        [HttpPost]
        [Route("updateProfile")]
        public HttpResponseMessage UpdateProfile(HttpRequestMessage request, UserDto userDto) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid) {
                    response = this.CreateModelValidationErrorResponse(request);
                }
                else {
                    this.userAppService.UpdateProfile(userDto);
                    response = request.CreateResponse(HttpStatusCode.OK, userDto);
                }

                return response;
            });
        }

        public HttpResponseMessage Delete(HttpRequestMessage request, Int64 id) {

            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                this.userAppService.DeleteById(id);

                response = request.CreateResponse(HttpStatusCode.OK, id);
                return response;
            });
        }
    }
}