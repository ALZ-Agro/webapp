using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ExpenseReportDto:ExpenseListDto {
        public string Company { get; set; }
        public string CompanyGroup { get; set; }
        /// <summary>
        /// Valor de la linea actual en la que se divide el item.
        /// En items con alícuota de iva distinta a 0
        /// sería el Total restado el tipo de concepto de la línea.
        /// </summary>
        public string InterfaceValue { get; set; }
        /// <summary>
        /// El código seteado al usuario.
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// Numero de la línea en la que se divide el item. 
        /// Si es un item sin iva, esto siempre va a ser 1, 
        /// caso contrario, es un número que va de 1 a n.
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// Si es gravado o no gravado.
        /// </summary>
        public string TaxType { get; set; }
    }
}
