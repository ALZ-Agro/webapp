using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {
    public class ExpenseConfiguration : EntityBaseConfiguration<Expense> {

        public ExpenseConfiguration() {

            Property(u => u.CategoryId).IsRequired();
            Property(u => u.Receipt).IsRequired();
            Property(u => u.Date).IsRequired();
            Property(u => u.PaymentId).IsRequired();
            Property(u => u.AliquotId).IsRequired();
            Property(u => u.Total).IsRequired();
            Property(u => u.CategoryId).IsRequired();
            Property(u => u.Notes).HasMaxLength(120);

        }
    }
}
