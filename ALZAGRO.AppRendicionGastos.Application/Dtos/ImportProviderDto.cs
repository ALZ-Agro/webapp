using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ImportProviderDto {
        public long Id { get; set; }
        public string LegalName { get; set; }
        public string Cuit { get; set; }
        public string LegalCondition { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactFullName { get; set; }
        public bool SyncedWithERP { get; set; }
    }
}
