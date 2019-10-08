using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.Criteria {
    public class ListViewCriteriaBase {

        public String UserName { get; set; }

        public Int32 Page { get; set; }

        public Int32 Size { get; set; }

        public String PartialDescription { get; set; }

        public String OrderBy { get; set; }

        public Int64 UserId { get; set; }

        //public Boolean? OrderByDescending { get; set; }
    }
}
