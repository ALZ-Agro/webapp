using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Criterias {
    public class ExpenseListViewCriteria : ListViewCriteriaBase {

        public long SyncStatusId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public long Exported { get; set; }
        public long ProviderId { get; set; }
        public long AliquotId { get; set; }
        public string FileName { get; set; }
        public bool GetForBackEnd { get; set; }
        public long PaymentId { get; set; }
        public long ExpenseId { get; set; }
        public Nullable<DateTime> DateOfExportation { get; set; }
        public long CategoryId { get; set; }
    }
}