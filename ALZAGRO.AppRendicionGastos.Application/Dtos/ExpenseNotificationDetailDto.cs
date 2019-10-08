using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExpenseNotificationDetailDto:IDto {
        public long Id { get; set; }
        public string Provider { get; set; }
        public string Payment { get; set; }
        public float Total { get; set; }
        public string Category { get; set; }
    }
}
