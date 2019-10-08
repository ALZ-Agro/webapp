using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class ProviderConfiguration : EntityBaseConfiguration<Provider> {

        public ProviderConfiguration() {

            Property(u => u.Email).HasMaxLength(200);
            Property(u => u.LegalName).IsRequired().HasMaxLength(200);
            Property(u => u.LegalConditionId).IsRequired();

        }
    }
}
