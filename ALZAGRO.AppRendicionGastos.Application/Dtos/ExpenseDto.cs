using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExpenseDto : IDto {
        public long Id { get; set; }

        public long UserId { get; set; }

        public UserDto User { get; set; }
        
        public CategoryDto Category { get; set; }

        public long CompanyId { get; set; }
        
        public SyncStatusDto SyncStatus { get; set; }
        
        public ProviderDto Provider { get; set; }
        
        public AliquotDto Aliquot { get; set; }
        
        public PaymentDto Payment { get; set; }
        
        public string Notes { get; set; }
        public string Total { get; set; }
        public string NetValue { get; set; }
        public string Receipt { get; set; }
        public DateTime Date { get; set; }
        public string IVA { get; set; }
        public string NotTaxedConcepts { get; set; }
        public long VehiculeMileage { get; set; }
        public bool Exported { get; set; }

        public string Group { get; set; }
        public Nullable<DateTime> ExportedDateTime { get; set; }

        public List<ImageDto> Images { get; set; }
        public List<ExpenseStatusesLogDto> Logs { get; set; }
        public long UpdatedBy { get; set; }
    }
}