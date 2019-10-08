using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Linq;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using AutoMapper;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/changeReason")]
    public class ChangeReasonController : ApiControllerBase {

        private readonly IEntityBaseRepository<RefusalReason> refusalRepository;
        private readonly IEntityBaseRepository<ApprovalReason> approvalRepository;
        private readonly IUnitOfWork unitOfWork;

        public ChangeReasonController(
                         IEntityBaseRepository<RefusalReason> refusalRepository,
                         IUnitOfWork unitOfWork,
                         IEntityBaseRepository<ApprovalReason> approvalRepository,
        IErrorAppService errorAppService)
            : base(errorAppService) {
            this.unitOfWork = unitOfWork;
            this.refusalRepository = refusalRepository;
            this.approvalRepository = approvalRepository;
        }


        [HttpGet]
        [Route("refusalReasons")]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.refusalRepository.GetAll().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }
        [HttpGet]
        [Route("approvalReasons")]
        public HttpResponseMessage GetApprovalReasons(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.approvalRepository.GetAll().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        //[HttpPost]
        //public HttpResponseMessage Post(HttpRequestMessage request, RefusalReasonDto entity) {
        //    return CreateHttpResponse(request, () => {
        //        HttpResponseMessage response = null;
        //        if (ModelState.IsValid) {
        //            var refusal = Mapper.Map<RefusalReasonDto,RefusalReason>(entity);
        //            if (entity.Id == 0) {
        //                refusalRepository.Add(refusal);
        //            } else {
        //                refusalRepository.Edit(refusal);
        //            }
        //            this.unitOfWork.Commit();
        //            response = request.CreateResponse(HttpStatusCode.OK);
        //        } else {
        //            response = request.CreateResponse(HttpStatusCode.BadRequest);
        //        }
        //        return response;
        //    });
        //}

    }
}