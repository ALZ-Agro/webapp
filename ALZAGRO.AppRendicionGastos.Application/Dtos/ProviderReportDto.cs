using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ProviderReportDto{
        public string UserFullName { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string LegalName { get; set; }
        public long Cuit { get; set; }
        public LegalConditionDto LegalCondition { get; set; }
        public string ContactFullName { get; set; }
    }
}
