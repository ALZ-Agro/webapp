using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class CompanyDto : IDto {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
