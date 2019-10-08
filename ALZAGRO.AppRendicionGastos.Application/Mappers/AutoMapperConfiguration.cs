using AutoMapper;

namespace ALZAGRO.AppRendicionGastos.WebUI.Mappings {

    public class AutoMapperConfiguration {

        public static void Configure() {
            Mapper.Initialize(x => {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<DtoToDomainMappingProfile>();
            });
        }
    }
}