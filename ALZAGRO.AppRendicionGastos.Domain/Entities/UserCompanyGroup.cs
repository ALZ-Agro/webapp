using Mvz.Fwk.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class UserCompanyGroup:EntityBase {
        public long UserId { get; set; }
        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        [ForeignKey("UserGroup")]
        public long UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
