using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVD01")]
    public class Distribution  {
        /// <summary>
        /// Identificador único del registro importado.
        /// Debe traer un número secuencial generado por la app.Relacionado a SAR_CORMVI_IDENTI
        /// </summary>
        //[Key]
        [Key, Column(Order = 0)]
        public long SAR_CORMVD01_IDENTI { get; set; }
        /// <summary>
        /// Identificador único del registro importado.                        
        /// Debe traer un número secuencial generado por la app.Relacionado a SAR_CORMVI_NROITM
        /// </summary>
        [Key, Column(Order = 1)]
        public int SAR_CORMVD01_NROITD { get; set; }
        /// <summary>
        /// Numero de sub-item                  
        /// Va a ser siempre tres líneas numeradas consecutivas del 1 al 3 para cada registro SAR_CORMVI_IDENTI
        /// </summary>
        [Key, Column(Order = 2)]
        public int SAR_CORMVD01_NROITM { get; set; }
        /// <summary>
        /// Dimension.                                                 
        /// Para el  SAR_CORMVD01_NROITD 1 va CCUN, para el _NROITD 2 va PRODUC y para el _NROITD 3 va REND
        /// </summary>
        public string SAR_CORMVD01_CODDIM { get; set; }
        /// <summary>
        /// Concatenar Empresa|Código (Ej: Al01|VE1)                                  
        /// Siempre las líneas que corresponden a SAR_CORMVD01_NROITD con valor 1 y 2 van en blanco.
        /// </summary>
        public string SAR_CORMVD01_CODDIS { get; set; }
    }
}
