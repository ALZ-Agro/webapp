using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface ICategoryAppService : IEntityBaseAppService<Category, CategoryDto>
    {
        SearchResultViewModel<CategoryDto> Search(ListViewCriteriaBase criteria);
    }
}