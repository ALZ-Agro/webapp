using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using AutoMapper;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers.Api {

    [RoutePrefix("api/config")]
    public class ConfigController:ApiControllerBase {
        private readonly IEntityBaseRepository<Config> configRepository;
        private readonly IEntityBaseRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public ConfigController(IEntityBaseRepository<Config> configRepository,
            IUnitOfWork unitOfWork,
            IEntityBaseRepository<User> userRepository,
                                IErrorAppService errorAppService): base(errorAppService) {
            this.configRepository = configRepository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Config, ConfigDto>(this.configRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()));

                return response;
            });
        }
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, ConfigDto dto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var currentUser = this.userRepository.GetAllIncluding(x => x.Role).Where(x => x.Id == (int)this.CurrentUserId).FirstOrDefault();
                if (currentUser != null) {
                    if(currentUser.Role.Description == "Administrador del sistema") {
                        if (dto.Id == 0) {
                            this.configRepository.Add(Mapper.Map<ConfigDto, Config>(dto));
                            dto.Id = this.configRepository.GetLastInsertId();
                        }
                        else {
                            this.configRepository.Edit(Mapper.Map<ConfigDto, Config>(dto));
                        }
                        this.unitOfWork.Commit();
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    } else {
                        response = request.CreateErrorResponse(HttpStatusCode.Forbidden, "No posees permiso para editar esta configuración.");
                    }
                } else {
                    response = request.CreateErrorResponse(HttpStatusCode.Forbidden, "Debés estar logueado para realizar esta solicitud.");
                }
                return response;
            });
        }
    }
}