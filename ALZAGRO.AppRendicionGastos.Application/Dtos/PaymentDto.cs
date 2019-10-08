using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class PaymentDto:IDto {
        public long Id { get; set; }
        public string Description { get; set; }
        public Nullable<long> UserId { get; set; }
        public UserDto User { get; set; }
    }
}
