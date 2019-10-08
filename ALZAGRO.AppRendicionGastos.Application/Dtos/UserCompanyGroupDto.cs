using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class UserCompanyGroupDto:IDto {
        public long Id { get; set; }
        public long UserId { get; set; }
        public CompanyDto Company { get; set; }
        public UserGroupDto UserGroup { get; set; }
    }
}
