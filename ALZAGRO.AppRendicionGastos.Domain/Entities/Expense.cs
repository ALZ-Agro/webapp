using Mvz.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Expense : EntityBase {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Aliquot")]
        public long AliquotId { get; set; }
        public Aliquot Aliquot { get; set; }

        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("SyncStatus")]
        public long SyncStatusId { get; set; }
        public SyncStatus SyncStatus { get; set; }

        [ForeignKey("Provider")]
        public long ProviderId { get; set; }
        public Provider Provider { get; set; }

        [ForeignKey("Payment")]
        public long PaymentId { get; set; }
        public Payment Payment { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        public string Group { get; set; }
        public string Notes { get; set; }
        public string Receipt { get; set; }
        public double Total { get; set; }
        public double NetValue { get; set; }
        public DateTime Date { get; set; }
        public double IVA { get; set; }
        public double NotTaxedConcepts { get; set; }
        public long VehiculeMileage { get; set; }
        public bool Exported { get; set; }
        public Nullable<DateTime> ExportedDateTime { get; set; }


        public ICollection<Image> Images { get; set; }
        public ICollection<ExpenseStatusLog> Logs { get; set; }
    }
}