using ALZAGRO.Reports.Data.Interfaces;

namespace ALZAGRO.Reports.Data {
    public class ReportsUnitOfWork : IReportsUnnitOfWork {

        private IReportContext DbContext { get; set; }

        public ReportsUnitOfWork(IReportContext dbContext) {
            this.DbContext = dbContext;
        }
        public void Commit() {
            DbContext.Commit();
        }
    }
}
