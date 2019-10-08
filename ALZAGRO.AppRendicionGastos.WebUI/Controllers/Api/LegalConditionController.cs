using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/legalCondition")]
    public class LegalConditionController : ApiControllerBase {
        
        private readonly IEntityBaseRepository<LegalCondition> legalConditionRepository;

        public LegalConditionController(
                                 IEntityBaseRepository<LegalCondition> legalConditionRepository,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.legalConditionRepository = legalConditionRepository;
        }


        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.legalConditionRepository.GetAll();
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }
    }
}