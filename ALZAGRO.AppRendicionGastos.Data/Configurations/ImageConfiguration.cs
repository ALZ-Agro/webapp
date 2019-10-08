using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class ImageConfiguration : EntityBaseConfiguration<Image> {

        public ImageConfiguration() {

            Property(u => u.ExpenseId).IsRequired();
            Property(u => u.Path).IsRequired().HasMaxLength(200);

        }
    }
}
