using Mvz.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class SalesGroupCompanyCategory: EntityBase {
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string SalesGroup { get; set; }
        public string CategoryDescription { get; set; }
        public string IVAType { get; set; }
        public string Company { get; set; }
    }
}
