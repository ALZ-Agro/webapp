using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure {

    public abstract class BaseDbContext : DbContext, IDbContext {

        public BaseDbContext() {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual void Commit() {
            try {
                base.SaveChanges();
            }
            catch (DbEntityValidationException e) {
                var errorMessages = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors) {
                    foreach (var ve in eve.ValidationErrors) {
                        errorMessages.AppendLine((String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                                ve.PropertyName, ve.ErrorMessage)));
                    }
                }
                throw new Exception(errorMessages.ToString());
            }
            catch (Exception ex) {
                throw this.ProcessException(ex); 
            }
        }

        public abstract Exception ProcessException(Exception ex);

        public void ExecuteSP(String sp) {
            this.Database.ExecuteSqlCommand("exec " + sp);
        }
    }
}