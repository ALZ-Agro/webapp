using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class UserCompanyDto : IDto {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public string Company { get; set; }
    }
}
