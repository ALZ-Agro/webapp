using Mvz.Fwk.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Aliquot:EntityBase {
        public string Description { get; set; }
        public double Value { get; set; }
    }
}
