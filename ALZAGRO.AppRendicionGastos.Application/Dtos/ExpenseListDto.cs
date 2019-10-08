using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExpenseListDto {
        public long Id { get; set; }
        public string SyncStatus { get; set; }
        public DateTime Date { get; set; }
        public string Provider { get; set; }
        public string PaymentType { get; set; }
        public string Payment { get; set; }
        public string Category { get; set; }
        public string Total { get; set; }
        public bool Exported { get; set; }
        public Nullable<DateTime> ExportedDateTime { get; set; }
        public SearchUserDto User { get; set; }
        public long PaymentId { get; set; }

        public string Group { get; set; }

        public string ProviderCuit { get; set; }
        public string Receipt { get; set; }
        public string NotTaxedConcepts { get; set; }
        public long VehiculeMileage { get; set; }
        public string NetValue { get; set; }
        public string IVA { get; set; }
        public string Notes { get; set; }
    }
}
