using AutoMapper;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods;

namespace ALZAGRO.AppRendicionGastos.WebUI.Mappings {

    public class DtoToDomainMappingProfile : Profile
    {

        public override string ProfileName {
            get { return "DtoToDomainMappings"; }
        }

        protected override void Configure()
        {
            
            Mapper.CreateMap<UserDto, User>()
                .ForMember(c => c.Role, m => m.UseValue(null))
                .ForMember(c => c.RoleId, m => m.MapFrom(s => s.Role.Id))
                .ForMember(c => c.UserCompanyGroups, m => m.UseValue(null));
            Mapper.CreateMap<PaymentDto, Payment>()
                .ForMember(c => c.User, m => m.UseValue(null))
                .ForMember(c => c.UserId, m => m.MapFrom(s => s.UserId));
            Mapper.CreateMap<ProviderDto, Provider>()
                .ForMember(c => c.LegalCondition, m => m.UseValue(null))
                .ForMember(c => c.User, m => m.UseValue(null))
                .ForMember(c => c.LegalConditionId, m => m.MapFrom(s => s.LegalCondition.Id));
            Mapper.CreateMap<ProviderDto, ProviderReportDto>();
            Mapper.CreateMap<CategoryDto, Category>();
            Mapper.CreateMap<AliquotDto, Aliquot>();
            Mapper.CreateMap<UserCompanyGroupDto, UserCompanyGroup>()
                .ForMember(c => c.Company, m => m.UseValue(null))
                .ForMember(c => c.UserGroup, m => m.UseValue(null))
                .ForMember(c => c.UserGroupId, m => m.MapFrom(s => s.UserGroup.Id))
                .ForMember(c => c.CompanyId, m => m.MapFrom(s => s.Company.Id));
            Mapper.CreateMap<UserGroupDto, UserGroup>();
            Mapper.CreateMap<CategoryDto, Category>();
            Mapper.CreateMap<UserCompanyDto, UserCompany>()
                .ForMember(c => c.Company, m => m.UseValue(null))
                .ForMember(c => c.User, m => m.UseValue(null));
            Mapper.CreateMap<ExpenseStatusesLogDto, ExpenseStatusLog>()
                 .ForMember(c => c.Expense, m => m.UseValue(null))
                 .ForMember(c => c.User, m => m.UseValue(null));
            Mapper.CreateMap<ExpenseDto, Expense>()
              .ForMember(c => c.Images, m => m.UseValue(null))
              .ForMember(c => c.Logs, m => m.UseValue(null))
              .ForMember(c => c.Aliquot, m => m.UseValue(null))
              .ForMember(c => c.Category, m => m.UseValue(null))
              .ForMember(c => c.Company, m => m.UseValue(null))
              .ForMember(c => c.Payment, m => m.UseValue(null))
              .ForMember(c => c.Provider, m => m.UseValue(null))
              .ForMember(c => c.SyncStatus, m => m.UseValue(null))
              .ForMember(c => c.User, m => m.UseValue(null))
              .ForMember(c => c.Total, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.Total)))
              .ForMember(c => c.NetValue, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.NetValue)))
              .ForMember(c => c.IVA, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.IVA)))
              .ForMember(c => c.NotTaxedConcepts, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.NotTaxedConcepts)))
              .ForMember(c => c.UserId, m=> m.MapFrom(s => s.UserId))
              .ForMember(c => c.AliquotId, m => m.MapFrom(s => s.Aliquot.Id))
              .ForMember(c => c.CategoryId, m => m.MapFrom(s => s.Category.Id))
              .ForMember(c => c.PaymentId, m => m.MapFrom(s => s.Payment.Id))
              .ForMember(c => c.ProviderId, m => m.MapFrom(s => s.Provider.Id))
              .ForMember(c => c.SyncStatusId, m => m.MapFrom(s => s.SyncStatus.Id));
            Mapper.CreateMap<ImageDto, Image>();
            Mapper.CreateMap<RefusalReasonDto, RefusalReason>();
            Mapper.CreateMap<ApprovalReasonDto, ApprovalReason>();
            Mapper.CreateMap<CompanyDto, Company>();
            Mapper.CreateMap<ConfigDto, Config>();
            Mapper.CreateMap<SyncStatusDto, SyncStatus>();
            Mapper.CreateMap<DeviceDto, Device>()
                .ForMember(c => c.User, m => m.UseValue(null));
            Mapper.CreateMap<NotificationDto, Notification>()
                .ForMember(c => c.User, m => m.UseValue(null))
                .ForMember(c => c.Device, m => m.UseValue(null))
                .ForMember(p => p.DeviceId, m => m.MapFrom(s => s.Device.Id))
                .ForMember(p => p.UserId, m => m.MapFrom(s => s.User.Id))
                .ForMember(c => c.Role, m => m.UseValue(null))
                .ForMember(p => p.RoleId, m => m.MapFrom(s => s.Role.Id));
            Mapper.CreateMap<RoleDto, Role>();
            Mapper.CreateMap<ExpenseDto, ExpenseListDto>()
                .ForMember(c => c.Category, m => m.MapFrom(s => s.Category.Description))
                .ForMember(c => c.Date, m => m.MapFrom(s => s.Date))
                .ForMember(c => c.PaymentType, m => m.MapFrom(s => s.Aliquot.Description))
                .ForMember(c => c.Exported, m => m.MapFrom(s => s.Exported))
                .ForMember(c => c.Id, m => m.MapFrom(s => s.Id))
                .ForMember(c => c.Payment, m => m.MapFrom(s => s.Payment.Description))
                .ForMember(c => c.Provider, m => m.MapFrom(s => s.Provider.LegalName))
                .ForMember(c => c.SyncStatus, m => m.MapFrom(s => s.SyncStatus.Description))
                .ForMember(c => c.Total, m => m.MapFrom(s => s.Total))
                .ForMember(c => c.ProviderCuit, m => m.MapFrom(s => s.Provider.Cuit))
                .ForMember(c => c.User, m => m.MapFrom(s => Mapper.Map<UserDto, SearchUserDto>(s.User)))
                .ForMember(c => c.PaymentId, m => m.MapFrom(s => s.Payment.Id));
            Mapper.CreateMap<LegalConditionDto, LegalCondition>();
            Mapper.CreateMap<UserDto, SearchUserDto>()
                .ForMember(c => c.FullName, m => m.MapFrom(s => s.FirstName + " " + s.LastName));
            Mapper.CreateMap<ProviderDto, ExportProviderDto>().
                ForMember(c => c.LegalCondition, m => m.MapFrom(s => s.LegalCondition.Description));
        }
    }
}