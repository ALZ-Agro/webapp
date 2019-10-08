using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class PaymentConfiguration : EntityBaseConfiguration<Payment> {

        public PaymentConfiguration() {

            Property(u => u.Description).HasMaxLength(100);
        }
    }
}
