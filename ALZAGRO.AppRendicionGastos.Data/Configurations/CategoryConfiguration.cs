using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class CompanyConfiguration : EntityBaseConfiguration<Company> {

        public CompanyConfiguration() {

            Property(u => u.Name).HasMaxLength(200);

        }
    }
}
