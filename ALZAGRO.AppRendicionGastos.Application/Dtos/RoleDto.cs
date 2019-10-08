using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class RoleDto:IDto {
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
