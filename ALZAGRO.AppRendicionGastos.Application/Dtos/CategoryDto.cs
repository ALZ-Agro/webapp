using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class CategoryDto:IDto {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; }
        /// <summary>
        /// Options:
        /// 0: Show when any provider is selected.
        /// 1: Show only when generic provider is selected
        /// 2: Show only when non generic provider is selected.
        /// </summary>
        public long ShowTo { get; set; }

        public string Code { get; set; }
    }
}
