using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Criterias {
    public class StatusChangeListCriteria : ListViewCriteriaBase {

        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string FileName { get; set; }
    }
}