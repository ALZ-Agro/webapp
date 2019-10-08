using Mvz.Fwk.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class UserCompany : EntityBase {
        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

    }
}
