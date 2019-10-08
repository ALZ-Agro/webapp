using Mvz.Fwk.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Payment:EntityBase {
        public string Description { get; set; }
        [ForeignKey("User")]
        public Nullable<long> UserId { get; set; }
        public User User { get; set; }
    }
}
