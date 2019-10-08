using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Criterias {
    public class UserListViewCriteria : ListViewCriteriaBase {

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public long RoleId { get; set; }
    }
}