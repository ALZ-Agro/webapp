using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ChangeExpenseStatusDto {
        public long Id { get; set; }
        public SyncStatusDto SyncStatus { get; set; }
        public string ChangeStatusReason { get; set; }
        public string ChangeStatusNote { get; set; }
    }
}
