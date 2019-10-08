using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface IProviderAppService : IEntityBaseAppService<Provider, ProviderDto>
    {
        object Search(ProviderListViewCriteria criteria);
        SearchResultViewModel<ProviderDto> ExportData(ProviderListViewCriteria criteria, bool? SyncedWithERP, List<long> providersToInclude);
        List<ExportProviderDto> Import(List<ProviderDto> providers);
    }
}