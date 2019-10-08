using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class DeviceDto : IDto {
        public long Id { get; set; }

        public string ApplicationToken { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }

        public string DeviceType { get; set; }

        public string InstallationId { get; set; }
        public List<string> Tags { get; set; }

        public DeviceDto() {
            Tags = new List<string>();
        }
    }
}
