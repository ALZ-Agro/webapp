using AutoMapper;
namespace ALZAGRO.Reports.Data.Mappings {

    public class AutoMapperConfiguration {

        public static void Configure() {
            Mapper.Initialize(x => {
                x.AddProfile<DomainToExternalSourceProfile>();
            });
        }
    }
}