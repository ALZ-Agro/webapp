using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities {
    public class Error : IEntityBase {

        public Int64 Id { get; set; }

        public String Message { get; set; }

        public String StackTrace { get; set; }

        public DateTime DateCreated { get; set; }
    }
}