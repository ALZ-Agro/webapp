

using ALZAGRO.Reports.Data.Interfaces;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ALZAGRO.Reports.Data {

    public class ReportRepository<T> :IReportRepository<T>
            where T : class, new() {

        protected IReportContext DataContext { get; set; }

        #region Constructor


        public ReportRepository(IReportContext dbContext) {
            DataContext = dbContext;
        }

        #endregion

      
        public virtual void Add(T entity) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entity);
                DataContext.Set<T>().Add(entity);
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public virtual IQueryable<T> GetAll() {
            return DataContext.Set<T>();
        }

        public virtual void Remove(T entity) {
            try {
                DbEntityEntry dbEntityEntry = DataContext.Entry<T>(entity);
                DataContext.Set<T>().Remove(entity);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }


    }
}