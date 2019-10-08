using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using Mvz.Fwk.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class ExpenseStatusLog:EntityBase, IEntityBase {
        public string Change { get; set; }
        public string ReasonOfChange { get; set; }
        public string Notes { get; set; }
        [ForeignKey("Expense")]
        public long ExpenseId { get; set; }
        public Expense Expense { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User{ get; set; }
    }
}
