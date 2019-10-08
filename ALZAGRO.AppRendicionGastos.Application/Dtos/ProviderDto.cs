using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ProviderDto:IDto{
        public long Id { get; set; }
        public string LegalName { get; set; }
        public long Cuit { get; set; }
        public LegalConditionDto LegalCondition { get; set; }
        public string Email { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public long PhoneNumber { get; set; }
        public string ContactFullName { get; set; }
        public bool SyncedWithERP { get; set; }
        public string UserFullName { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public Nullable<long> UserId { get; set; }
    }
}
