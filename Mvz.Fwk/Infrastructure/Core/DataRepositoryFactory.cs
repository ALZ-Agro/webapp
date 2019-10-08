using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.WebUI.Infrastructure.Extensions;
using System.Net.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.Infrastructure.Core {

    public class DataRepositoryFactory : IDataRepositoryFactory {

        public IEntityBaseRepository<T> GetDataRepository<T>(HttpRequestMessage request)
            where T : class, IEntityBase, new() {

            return request.GetDataRepository<T>();
        }
    }

    public interface IDataRepositoryFactory {
        IEntityBaseRepository<T> GetDataRepository<T>(HttpRequestMessage request)
            where T : class, IEntityBase, new();
    }
}