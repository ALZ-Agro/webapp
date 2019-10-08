using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVI07")]
    public class TaxesDetails {
        [Key, Column(Order = 0)]
        public string SAR_CORMVI07_IDENTI { get; set; }
        [Key, Column(Order = 1)]
        public string SAR_CORMVI07_TIPCPT { get; set; }
        [Key, Column(Order = 2)]
        public string SAR_CORMVI07_CODCPT { get; set; }
        public string SAR_CORMVI07_INGRES { get; set; }
        public int SAR_CORMVI07_NROITM { get; set; }
        public string SAR_CORMVI07_IMPGRA { get; set; }
        public string SAR_CORMVI07_PORCEN { get; set; }
        public TaxesDetails() {
            this.SAR_CORMVI07_TIPCPT = "I";
        }
    }
}
