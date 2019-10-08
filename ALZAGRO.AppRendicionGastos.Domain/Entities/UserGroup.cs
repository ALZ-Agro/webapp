using System.ComponentModel.DataAnnotations;
using Mvz.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class UserGroup:EntityBase {
        public string Description { get; set; }

        [Required, MaxLength(1)]
        public string Code { get; set; }
    }
}
