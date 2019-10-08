using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVI09")]
    public class H_Concepts  {
        [Key, Column(Order = 0)]
        public long SAR_CORMVI09_IDENTI { get; set; }
        public string SAR_CORMVI09_CODCPT { get; set; }
        public string SAR_CORMVI09_TIPCPT { get; set; }
        [Key, Column(Order = 1)]
        public long SAR_CORMVI09_NROITM { get; set; }
        public string SAR_CORMVI09_IMPORT { get; set; }
        /// <summary>
        /// OPTIONALSs
        /// </summary>
        public string SAR_CORMVI09_CHEQUE { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_CODBCO { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_TIPDOC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_NRODOC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_FCHVNC { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_DOCFIR { get; set; }
        /// <summary>
        /// OPTIONALS
        /// </summary>
        public string SAR_CORMVI09_TEXTOS { get; set; }

        public H_Concepts() {
            this.SAR_CORMVI09_NROITM = 1;
            this.SAR_CORMVI09_TIPCPT = "F";
        }
    }
}
