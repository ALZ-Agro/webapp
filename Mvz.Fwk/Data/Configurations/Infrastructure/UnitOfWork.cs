using ALZAGRO.AppRendicionGastos.Fwk.Domain;

namespace ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure {

    public class UnitOfWork : IUnitOfWork {

        private IDbContext DbContext { get; set; }

        public UnitOfWork(IDbContext dbContext) {
            this.DbContext = dbContext;
        }
        
        public void Commit() {
            DbContext.Commit();
        }
    }
}