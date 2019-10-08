
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using AutoMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers.Api {
    [RoutePrefix("api/device")]
    public class DeviceController : ApiControllerBase {

        private readonly IEntityBaseRepository<Device> deviceRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeviceController(IUnitOfWork unitOfWork,
                                 IEntityBaseRepository<Device> deviceRepository,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.unitOfWork = unitOfWork;
            this.deviceRepository = deviceRepository;
        }

        [HttpPost]
        [Route("sync")]
        public HttpResponseMessage Post(HttpRequestMessage request, DeviceDto deviceDto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                if (ModelState.IsValid) {
                    var device = Mapper.Map<DeviceDto, Device>(deviceDto);
                    if (deviceDto.Id == 0) {
                        this.deviceRepository.Add(device);
                        deviceDto.Id = this.deviceRepository.GetLastInsertId();
                    } else {
                        this.deviceRepository.Edit(device);
                    }
                    this.unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, id = deviceDto.Id });
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }

                return response;
            });
        }
    }
}