using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExportExpenseDto : ExpenseDto {
        public string ItemNumber { get; set; }
        public string InterfaceValue { get; set; }
        public string TaxType { get; set; }
    }
}
