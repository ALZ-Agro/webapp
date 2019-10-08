using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ALZAGRO.AppRendicionGastos.Fwk.Data.Configurations {

    public class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntityBase {

        public EntityBaseConfiguration() {
            HasKey(e => e.Id);
        }
    }
}