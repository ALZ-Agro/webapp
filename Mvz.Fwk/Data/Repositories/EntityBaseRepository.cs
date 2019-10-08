using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALZAGRO.AppRendicionGastos.Fwk.Data.Repositories {

    public class IEntityBaseRepository<T> : Domain.IEntityBaseRepository<T>
            where T : class, IEntityBase, new() {

        protected IDbContext DataContext { get; set; }

        #region Constructor


        public IEntityBaseRepository(IDbContext dbContext) {
            DataContext = dbContext;
        }

        #endregion

        public virtual IQueryable<T> GetAll() {
            return DataContext.Set<T>();
        }


        //TODO: Pendiente hasta que implementemos Auth2.0
        public Int64? CurrentUserId {
            get {

                var customPrincipal = HttpContext.Current.User as CustomPrincipal;
                if (customPrincipal != null) {
                    return customPrincipal.UserId;
                }
                else {
                    return null;
                }
            }
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, Object>>[] includeProperties) {
            IQueryable<T> query = DataContext.Set<T>().AsNoTracking();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T GetSingle(Int64 id) {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public Int64 GetLastInsertId() {
            return DataContext.Set<T>().Max(x => x.Id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, Boolean>> predicate) {
            return DataContext.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entity);
                DataContext.Set<T>().Add(entity);
            }
            catch (Exception ex) {
                this.DataContext.ProcessException(ex);
            }
        }

        public virtual void AddRange(List<T> entities) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entities.First());
                DataContext.Set<T>().AddRange(entities);
            }
            catch (Exception ex) {
                this.DataContext.ProcessException(ex);
            }
        }

        public virtual void Edit(T entity) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception ex) {
                try {
                    var original = DataContext.Set<T>().Find(entity.Id);
                    DbEntityEntry dbEntityEntry = DataContext.Entry<T>(original);
                    dbEntityEntry.CurrentValues.SetValues(entity);
                    dbEntityEntry.State = EntityState.Modified;
                }
                catch {
                    this.DataContext.ProcessException(ex);
                }

            }
        }

        public virtual void Delete(T entity) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            catch (Exception ex) {
                this.DataContext.ProcessException(ex);
            }
        }

        public void ExecuteSP(String sp) {
            this.DataContext.ExecuteSP(sp);
        }
    }
}