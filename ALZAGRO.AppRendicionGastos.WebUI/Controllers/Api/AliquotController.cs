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

    [RoutePrefix("api/aliquot")]
    public class AliquotController : ApiControllerBase {

        private readonly IEntityBaseRepository<Aliquot> aliquotRepository;


        public AliquotController(
                                 IEntityBaseRepository<Aliquot> aliquotRepository,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.aliquotRepository = aliquotRepository;
        }

        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<List<Aliquot>, List<AliquotDto>>(this.aliquotRepository.GetAll().ToList()));

                return response;
            });
        }
    }
}