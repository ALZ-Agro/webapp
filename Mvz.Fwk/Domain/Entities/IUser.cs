using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities {

    public interface IUser : IEntityBase {

        String Username { get; set; }

    }
}