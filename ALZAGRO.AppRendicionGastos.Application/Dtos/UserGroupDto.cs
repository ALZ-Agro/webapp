using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;


namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class UserGroupDto:IDto {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

    }
}
