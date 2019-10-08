using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.Reports.Data.Entities {
    [Table("SAR_CORMVH")]
    public class ReceiptHeader {
        /// <summary>
        /// Identificador
        /// Identificador único del registro importado. El dato debe ser generado por el sistema origen.
        /// </summary>
        [Key]
        public string SAR_CORMVH_IDENTI { get; set; }
        /// <summary>
        /// Status
        /// "Valor por defecto N. Esta campo informa el resultado del procesamiento del registro:
        /// S: Procesado correctamente
        /// E: Con error
        /// N: Pendiente de procesar"
        /// </summary>
        public char SAR_CORMVH_STATUS { get; set; }
        /// <summary>
        /// Circuito a generar
        /// Codigo de circuito en el que debe impactar la registracion
        /// </summary>
        public string SAR_CORMVH_CIRCOM { get; set; }
        /// <summary>
        /// Circuito del que parte
        /// Codigo de circuito en el que debe impactar la registracion
        /// </summary>
        public string SAR_CORMVH_CIRAPL { get; set; }
        /// <summary>
        /// Módulo
        /// En caso de que se desee generar un comprobante forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de compras generado.
        /// </summary>
        public string SAR_CORMVH_MODFOR { get; set; }
        /// <summary>
        /// Código
        /// En caso de que se desee generar un comprobante forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de compras generado.
        /// </summary>
        public string SAR_CORMVH_CODFOR { get; set; }
        /// <summary>
        /// Número
        /// En caso de que se desee generar un comprobante forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de compras generado.
        /// </summary>
        public int? SAR_CORMVH_NROFOR { get; set; }
        /// <summary>
        /// Código de Empresa
        /// En caso de esquema multisociedad, indicar aquí la empresa en la que se desea que impacte el registro. En un esquema multiempresa, dejar nulo este valor.
        /// </summary>
        public string SAR_CORMVH_CODEMP { get; set; }
        /// <summary>
        /// Proveedor
        /// OBLIGATORIO
        /// </summary>
        public string SAR_CORMVH_NROCTA { get; set; }
        /// <summary>
        /// Deposito de Origen
        /// </summary>
        public string SAR_CORMVH_DEPOSI { get; set; }
        /// <summary>
        /// Sector
        /// </summary>
        public string SAR_CORMVH_SECTOR { get; set; }
        /// <summary>
        /// Sector de requerimientos
        /// </summary>
        public string SAR_CORMVH_SECREQ { get; set; }
        /// <summary>
        /// Jurisdicción
        /// </summary>
        public string SAR_CORMVH_JURISD { get; set; }
        /// <summary>
        /// Código Original
        /// </summary>
        public string SAR_CORMVH_CODORI { get; set; }
        /// <summary>
        /// Código de Autorización de Impresión
        /// </summary>
        public string SAR_CORMVH_NROCAI { get;set;}
        /// <summary>
        /// Medio de Transporte
        /// </summary>
        public string SAR_CORMVH_EMBARQ { get;set;}
        /// <summary>
        /// Condición Comercial
        /// </summary>
        public string SAR_CORMVH_CNDCOM { get;set;}
        /// <summary>
        /// Comprador
        /// </summary>
        public string SAR_CORMVH_CMPRAD { get;set;}
        /// <summary>
        /// Error
        /// </summary>
        public string SAR_CORMVH_ERRMSG { get;set;}
        /// <summary>
        /// Empresa ST
        /// En caso de que se desee generar un comprobante de Stock forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de stock generado.
        /// </summary>
        public string SAR_CORMVH_EMPFST { get;set;}
        /// <summary>
        /// Módulo ST
        /// En caso de que se desee generar un comprobante de Stock forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de stock generado.
        /// </summary>
        public string SAR_CORMVH_MODFST { get;set;}
        /// <summary>
        /// Código ST
        /// En caso de que se desee generar un comprobante de Stock forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de stock generado.
        /// </summary>
        public string SAR_CORMVH_CODFST { get;set;}
        /// <summary>
        /// Número ST
        /// En caso de que se desee generar un comprobante de Stock forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de stock generado.
        /// </summary>
        public int? SAR_CORMVH_NROFST { get;set;}
        /// <summary>
        /// Empresa PV
        /// En caso de que se desee generar un comprobante de proveedores forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de proveedores generado.
        /// </summary>
        public string SAR_CORMVH_EMPFPV { get;set;}
        /// <summary>
        /// Módulo PV
        /// En caso de que se desee generar un comprobante de proveedores forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de proveedores generado.
        /// </summary>
        public string SAR_CORMVH_MODFPV { get;set;}
        /// <summary>
        /// Código PV
        /// En caso de que se desee generar un comprobante de proveedores forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de proveedores generado.
        /// </summary>
        public string SAR_CORMVH_CODFPV { get;set;}
        /// <summary>
        /// Número PV
        /// En caso de que se desee generar un comprobante de proveedores forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de proveedores generado.
        /// </summary>
        public int? SAR_CORMVH_NROFPV { get;set;}
        /// <summary>
        /// Empresa CJ
        /// En caso de que se desee generar un comprobante de tesorería forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de tesorería generado.
        /// </summary>
        public string SAR_CORMVH_EMPFCJ { get;set;}
        /// <summary>
        /// Módulo CJ
        /// En caso de que se desee generar un comprobante de tesorería forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de tesorería generado.
        /// </summary>
        public string SAR_CORMVH_MODFCJ { get;set;}
        /// <summary>
        /// Código CJ
        /// En caso de que se desee generar un comprobante de tesorería forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de tesorería generado.
        /// </summary>
        public string SAR_CORMVH_CODFCJ { get;set;}
        /// <summary>
        /// Número CJ
        /// En caso de que se desee generar un comprobante de tesorería forzando los datos del mismo, completar campos. De lo contrario, dejar nulos. Una vez procesado el registro, aquí se indicaran los datos del comprobante de tesorería generado.
        /// </summary>
        public int? SAR_CORMVH_NROFCJ { get;set;}
        /// <summary>
        /// Fecha de Contabilizacion del movimiento
        /// Fecha contable en la que impactará el movimiento
        /// </summary>
        public DateTime SAR_CORMVH_FCHMOV { get;set;}
        /// <summary>
        /// Fecha de emision del comprobante
        /// En el caso de importar facturas, aquí se indica la fecha de emision de la factura
        /// </summary>
        public string SAR_CORMVH_FCHEMI { get;set;}
        /// <summary>
        /// S = El sistema intentará procesar el registro automáticamente, ejecutando el job indicado en el campo _CODJOB
        /// N = El sistema no intentará procesar el registro automáticamente
        /// </summary>
        public char SAR_CORMVH_EJEAUT { get;set;}
        /// <summary>
        /// Indicar el código de job a procesar para importar automáticamente el registro. En caso de que el código ingresado sea inexistente, se informará en e campo _ERRMSG modificandose el estado de registro a E (Error)
        /// </summary>
        public string SAR_CORMVH_CODJOB { get;set;}

        /// <summary>
        /// Observación del usuario.
        /// </summary>
        public string SAR_CORMVH_TEXTOS { get; set; }
        public ReceiptHeader() {
            this.SAR_CORMVH_STATUS = 'N';
            this.SAR_CORMVH_EJEAUT = 'N';
        }
    }

    
}
