using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {

    public class LoginDto : IDto {

        public long Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}