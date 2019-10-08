using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExpenseStatusesLogDto:IDto {
        public long Id { get; set; }
        public string Change { get; set; }
        public string ReasonOfChange { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long ExpenseId { get; set; }
        public string ExpenseUploaderFullName { get; set; }

    }
}
