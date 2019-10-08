using Mvz.Fwk.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Image: EntityBase {
        [ForeignKey("Expense")]
        public long ExpenseId { get; set; }
        public Expense Expense { get; set; }
        public string Path { get; set; }
    }
}
