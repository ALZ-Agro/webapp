using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class NotificationDto : IDto {

        public long Id { get; set; }

        public DeviceDto Device { get; set; }

        public RoleDto Role { get; set; }

        public UserDto User { get; set; }

        public string Type { get; set; }

        public string Platform { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public string IconUrl { get; set; }

        public bool Read { get; set; }

        public string OnClick { get; set; }

        public string OnAppClick { get; set; }

        public string ClickParameter { get; set; }

        public DateTime UpdatedDateTime { get; set; }

    }
}
