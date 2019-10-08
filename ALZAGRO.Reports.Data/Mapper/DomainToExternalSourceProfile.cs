using AutoMapper;

namespace ALZAGRO.Reports.Data.Mappings {

    public class DomainToExternalSourceProfile : Profile {

        public override string ProfileName {
            get { return "DomainToExternalSourceProfile"; }
        }

        protected override void Configure() {
            //Mapper.CreateMap<UserDto, User>()
            //    .ForMember(c => c.Role, m => m.UseValue(null))
            //    .ForMember(c => c.RoleId, m => m.MapFrom(s => s.Role.Id))
            //    .ForMember(c => c.UserCompanyGroups, m => m.UseValue(null));
            
        }
    }
}