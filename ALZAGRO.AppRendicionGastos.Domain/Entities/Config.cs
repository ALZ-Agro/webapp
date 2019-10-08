using Mvz.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Config : EntityBase{
        public long SyncDays { get; set; }
        public long SyncIntervalInSeconds { get; set; }
    }
}
