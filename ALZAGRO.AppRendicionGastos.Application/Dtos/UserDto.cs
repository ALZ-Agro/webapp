using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {

    public class UserDto : IDto {

        public long Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public long RoleId { get; set; }
        public RoleDto Role { get; set; }

        public DateTime? LastActivityDateTime { get; set; }

        public bool IsLocked { get; set; }

        public bool ShowNotifications { get; set; }

        public List<UserCompanyGroupDto> UserCompanyGroups { get; set; }

        public string Id_Erp { get; set; }

        public List<UserCompanyGroupPlainDto> UserCompanyGroupsPlain { get; set; }

    }
}