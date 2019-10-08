using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Linq;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using ALZAGRO.AppRendicionGastos.Fwk.UI;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class CategoryAppService : EntityBaseAppService<Category, CategoryDto>, ICategoryAppService{
        public CategoryAppService(IEntityBaseRepository<Category> categoryRepository,
                             IEntityBaseRepository<Error> errorsRepository,
                             IUnitOfWork unitOfWork) :
            base(errorsRepository, unitOfWork, categoryRepository)
        {
        }

        public SearchResultViewModel<CategoryDto> Search(ListViewCriteriaBase criteria) {
            var query = this.entityRepository.GetAll();
            if (!string.IsNullOrEmpty(criteria.PartialDescription)) {
                query = this.MatchInFields<Category>(query, criteria.PartialDescription, true, c => new[] {
                    c.Description.ToLower()
                });
            }
            return this.CreateResult<Category, CategoryDto>(query, criteria, "Id");
        }
    }
}
