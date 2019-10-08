using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ConfigDto : IDto {
        public long Id { get; set; }
        public long SyncDays { get; set; }
        public long SyncIntervalInSeconds { get; set; }
    }
}
