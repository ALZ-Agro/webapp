using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class SearchProviderDto:IDto{
        public long Id { get; set; }
        public string LegalName { get; set; }
        public long Cuit { get; set; }
        public Nullable<long> CategoryId { get; set; }
    }
}
