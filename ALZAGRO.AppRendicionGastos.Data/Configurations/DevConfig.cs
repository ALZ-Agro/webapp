using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class DevConfig : EntityBaseConfiguration<Config> {

        public DevConfig() {

            Property(u => u.SyncDays).IsRequired();
            Property(u => u.SyncIntervalInSeconds).IsRequired();

        }
    }
}
