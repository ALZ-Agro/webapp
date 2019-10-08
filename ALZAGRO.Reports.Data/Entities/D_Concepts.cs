using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVI08")]
    public class D_Concepts {

        [Key, Column(Order = 0)]
        public long SAR_CORMVI08_IDENTI { get; set; }
        public string SAR_CORMVI08_CODCPT { get; set; }
        public string SAR_CORMVI08_TIPCPT { get; set; }
        [Key, Column(Order = 1)]
        public long SAR_CORMVI08_NROITM { get; set; }
        public string SAR_CORMVI08_IMPORT { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_CHEQUE { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_CODBCO { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_TIPDOC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_NRODOC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_FCHVNC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_DOCFIR { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI08_TEXTOS { get; set; }

        public D_Concepts() {
            this.SAR_CORMVI08_NROITM = 1;
            this.SAR_CORMVI08_TIPCPT = "Z";
            this.SAR_CORMVI08_CODCPT = "CTR004";
        }
    }
}
