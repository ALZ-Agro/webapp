using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Fwk.CrossCutting {
    public interface IErrorAppService : IEntityBaseAppService<Error> {

        void Log(Error error);
    }
}