using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVI")]
    public class ReceiptItem {
        /// <summary>
        /// Identy
        /// Identificador único del registro importado. Relacionado a CORMVH_IDENTI
        /// </summary>
        [Key, Column(Order = 0)]
        public string SAR_CORMVI_IDENTI { get; set; }
        /// <summary>
        /// Item
        /// Numero de item del comprobante
        /// </summary>
        [Key, Column(Order = 1)]
        public int SAR_CORMVI_NROITM { get; set; }
        /// <summary>
        /// Tipo de Producto
        /// </summary>
        public string SAR_CORMVI_TIPORI { get; set; }
        /// <summary>
        /// Producto
        /// </summary>
        public string SAR_CORMVI_ARTORI { get; set; }
        /// <summary>
        /// Cantidad
        /// </summary>
        public int SAR_CORMVI_CANTID { get; set; }
        /// <summary>
        /// Precio
        /// </summary>
        public string SAR_CORMVI_PRECIO { get; set; }
        /// <summary>
        /// Empresa
        /// En esquema multisociedad, se completa cuando el circuito es de aplicación de un paso anterior (CORMVH_CIRCOM distinto a CORMVH_CIRAPL) indicando la empresa del comprobante sobre el que aplica el registro. En esquema multiempresa, dejar nulo
        /// </summary>
        public string SAR_CORMVI_EMPAPL { get; set; }
        /// <summary>
        /// Módulo
        /// Se completa cuando el circuito es de aplicación de un paso anterior (CORMVH_CIRCOM distinto a CORMVH_CIRAPL). En caso contrario, dejar nulo.Indica modulo del comprobante sobre el que aplica la registracion.
        /// </summary>
        public string SAR_CORMVI_MODAPL { get; set; }
        /// <summary>
        /// Código
        /// Se completa cuando el circuito es de aplicación de un paso anterior (CORMVH_CIRCOM distinto a CORMVH_CIRAPL). En caso contrario, dejar nulo.Indica codigo del comprobante sobre el que aplica la registracion.
        /// </summary>
        public string SAR_CORMVI_CODAPL { get; set; }
        /// <summary>
        /// Número
        /// Se completa cuando el circuito es de aplicación de un paso anterior (CORMVH_CIRCOM distinto a CORMVH_CIRAPL). En caso contrario, dejar nulo.Indica numero del comprobante sobre el que aplica la registracion.
        /// </summary>
        public int? SAR_CORMVI_NROAPL { get; set; }
        /// <summary>
        /// Item
        /// Se completa cuando el circuito es de aplicación de un paso anterior (CORMVH_CIRCOM distinto a CORMVH_CIRAPL). En caso contrario, dejar nulo.Indica item del comprobante sobre el que aplica la registracion.
        /// </summary>
        public int? SAR_CORMVI_ITMAPL { get; set; }
    }
}
