using Mvz.Fwk.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Provider:EntityBase {
        public string LegalName { get; set; }
        public Nullable<long> Cuit { get; set; }
        [ForeignKey("LegalCondition")]
        public long LegalConditionId { get; set; }
        public LegalCondition LegalCondition { get; set; }
        public string Email { get; set; }
        public Nullable<long> PhoneNumber { get; set; }
        public string ContactFullName { get; set; }
        public bool SyncedWithERP { get; set; }
        public Nullable<long> CategoryId { get; set; }
        [ForeignKey("User")]
        public Nullable<long> UserId { get; set; }
        public User User { get; set; }
    }
}
