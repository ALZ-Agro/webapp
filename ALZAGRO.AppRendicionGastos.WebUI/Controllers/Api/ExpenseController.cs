using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using System;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/expense")]
    public class ExpenseController : ApiControllerBase {

        private readonly IExpenseAppService expenseAppService;
        private readonly IEntityBaseRepository<SyncStatus> syncStatusRepository;

        public ExpenseController(
                                 IExpenseAppService expenseAppService,
                                 IEntityBaseRepository<SyncStatus> syncStatusRepository,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.expenseAppService = expenseAppService;
            this.syncStatusRepository = syncStatusRepository;
        }

        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, [FromUri] Int64 Id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, this.expenseAppService.GetSingle(Id));
                return response;
            });
        }

        [Route("getExpenseUsersByCompanyId/{id}")]
        [HttpGet]
        public HttpResponseMessage GetExpenseUsers(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (id != 0) {
                    var items = expenseAppService.GetExpenseUsersByCompanyId(id);
                    response = request.CreateResponse(HttpStatusCode.OK, items);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }


                return response;
            });
        }

        [Route("search")]
        [HttpGet]
        public HttpResponseMessage Search(HttpRequestMessage request, [FromUri] ExpenseListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (criteria.CompanyId != 0) {
                    var items = this.expenseAppService.Search(criteria);
                    response = request.CreateResponse(HttpStatusCode.OK, items);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }
                
                
                return response;
            });
        }

        [Route("statusLog")]
        [HttpGet]
        public HttpResponseMessage GetStatusesLogForExpense(HttpRequestMessage request, [FromUri] ExpenseListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (criteria.ExpenseId != 0) {
                    var items = this.expenseAppService.GetStatusesLogOfExpense(criteria);
                    response = request.CreateResponse(HttpStatusCode.OK, items);
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }


                return response;
            });
        }

        [Route("statusLogs")]
        [HttpGet]
        public HttpResponseMessage GetStatusLogList(HttpRequestMessage request, [FromUri] ExpenseListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.expenseAppService.GetStatusLogList(criteria);
                response = request.CreateResponse(HttpStatusCode.OK, items);
                return response;
            });
        }

        [Route("analytics")]
        [HttpGet]
        public HttpResponseMessage GetAnalytics(HttpRequestMessage request, [FromUri] ExpenseListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                    response = request.CreateResponse(HttpStatusCode.OK, this.expenseAppService.GetAnalytics(criteria));
                return response;
            });
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetExpenseAdminList(HttpRequestMessage request, [FromUri] ExpenseListViewCriteria criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, this.expenseAppService.GetExpensesList(criteria));
                return response;
            });
        }

        [Route("statuses")]
        [HttpGet]
        public HttpResponseMessage GetStatuses(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = Mapper.Map<List<SyncStatus>, 
                                       List<SyncStatusDto>>(this.syncStatusRepository.GetAll().
                                                                                      Where(x => x.Description != "Duplicado").
                                                                                      ToList());
                response = request.CreateResponse(HttpStatusCode.OK, items);
                return response;
            });
        }

        [Route("changeStatus")]
        [HttpPost]
        public HttpResponseMessage Save(HttpRequestMessage request, ChangeExpenseStatusDto entity) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                    var status = this.expenseAppService.ChangeStatus(entity);
                    if (status.Success) {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    } else{
                        response = request.CreateResponse(HttpStatusCode.Forbidden, new { success = false, message = status.Message });
                    }
                   
                    return response;
            });
        }

        [Route("edit/{id}")]
        [HttpGet]
        public HttpResponseMessage Edit(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var status = this.expenseAppService.SetEditingState(id);
                if (status.Success) {
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else {
                    response = request.CreateResponse(HttpStatusCode.Forbidden, new { success = false, message = status.Message });
                }

                return response;
            });
        }

        [HttpPost]
        public HttpResponseMessage Save(HttpRequestMessage request, ExpenseDto expenseDto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                if (ModelState.IsValid) {
                    var expense = this.expenseAppService.Save(expenseDto);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, id = expense.Id });
                }
                else {
                    response = this.CreateModelValidationErrorResponse(request);
                }

                return response;
            });
        }
    }
}