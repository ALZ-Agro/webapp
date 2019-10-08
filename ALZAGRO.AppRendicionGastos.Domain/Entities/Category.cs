using System.ComponentModel.DataAnnotations;
using Mvz.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Category:EntityBase {
        public string Description { get; set; }
        public string ProductType { get; set; }
        /// <summary>
        /// Options:
        /// 0: Show when any provider is selected.
        /// 1: Show only when generic provider is selected
        /// 2: Show only when non generic provider is selected.
        /// </summary>
        public long ShowTo { get; set; }
        [MaxLength(2), Required]
        public string Code { get; set; }
    }
}
