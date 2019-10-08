using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CJRMVD10")]
    public class DistDimTesoH {
        [Key, Column(Order = 0)]
        public long SAR_CJRMVD10_IDENTI { get; set; }
        [Key, Column(Order = 1)]
        public string SAR_CJRMVD10_NROITM { get; set; }
        [Key, Column(Order = 2)]
        public string SAR_CJRMVD10_NROITD { get; set; }
        public string SAR_CJRMVD10_CODDIM { get; set; }
        public string SAR_CJRMVD10_CODDIS { get; set; }
        public DistDimTesoH() {
            this.SAR_CJRMVD10_NROITM = "1";
            this.SAR_CJRMVD10_NROITD = "1";
            this.SAR_CJRMVD10_CODDIM = "REND";
        }
    }
}
