using ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations;
using ALZAGRO.AppRendicionGastos.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Data.Configurations {

    public class UserConfiguration : EntityBaseConfiguration<User> {

        public UserConfiguration() {

            Property(u => u.Username).IsRequired().HasMaxLength(100);
            Property(u => u.Email).IsRequired().HasMaxLength(200);
            Property(u => u.HashedPassword).IsRequired().HasMaxLength(200);
            Property(u => u.Salt).IsRequired().HasMaxLength(200);
        }
    }
}