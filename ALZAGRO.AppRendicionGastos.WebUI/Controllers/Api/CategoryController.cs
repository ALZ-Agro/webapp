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
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using System;

namespace ALZAGRO.AppRendicionGastos.WebUI.Controllers {

    [RoutePrefix("api/category")]
    public class CategoryController : ApiControllerBase {
        private readonly ICategoryAppService categoryAppService;
        private readonly IEntityBaseRepository<Category> categoryRepository;

        public CategoryController(
                                 IEntityBaseRepository<Category> categoryRepository,
                                 ICategoryAppService categoryAppService,
                                 IErrorAppService errorAppService)
            : base(errorAppService) {
            this.categoryRepository = categoryRepository;
            this.categoryAppService = categoryAppService;
        }

        [Route("all")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<List<Category>,List<CategoryDto>>(this.categoryRepository.GetAll().ToList()));
                
                return response;
            });
        }
        [HttpGet]
        [Route("search")]
        public HttpResponseMessage GetPaymentsOf(HttpRequestMessage request, [FromUri] ListViewCriteriaBase criteria) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.categoryAppService.Search(criteria);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.categoryAppService.GetById(id);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        public HttpResponseMessage Post(HttpRequestMessage request, CategoryDto dto) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var items = this.categoryAppService.Save(dto);
                response = request.CreateResponse(HttpStatusCode.OK, items);

                return response;
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, Int64 id) {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                this.categoryAppService.DeleteById(id);
                response = request.CreateResponse(HttpStatusCode.OK);

                return response;
            });
        }
    }
}