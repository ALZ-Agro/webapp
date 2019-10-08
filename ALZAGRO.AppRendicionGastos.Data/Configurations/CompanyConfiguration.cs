using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class CategoryConfiguration : EntityBaseConfiguration<Category> {

        public CategoryConfiguration() {

            Property(u => u.Description).HasMaxLength(200);

        }
    }
}
