using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Criterias {
    public class ProviderListViewCriteria : ListViewCriteriaBase {

        public bool Exported { get; set; }
        public string FileName { get; set; }
        public bool GetForBackOffice { get; set; }
        public bool NotSyncedWithERP { get; set; }
        public long LegalConditionId { get; set; }
        public bool OverrideExportCheck { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long RoleId { get; set; }
        public long ExportStatus { get; set; }
    }
}