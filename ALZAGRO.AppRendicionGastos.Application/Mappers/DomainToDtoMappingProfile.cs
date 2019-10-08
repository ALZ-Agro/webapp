using AutoMapper;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods;
using System;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Fwk.UI;

namespace ALZAGRO.AppRendicionGastos.WebUI.Mappings
{

    public class DomainToViewModelMappingProfile : Profile
    {

        public override string ProfileName {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure() {
            Mapper.CreateMap<User, UserDto>()
               .ForMember(c => c.FullName, m => m.MapFrom(s => s.FirstName + " " + s.LastName))
               .ForMember(c => c.Role, m => m.MapFrom(s => Mapper.Map<Role, RoleDto>(s.Role)))
               .ForMember(c => c.Password, m => m.MapFrom(s => s.HashedPassword))
               .ForMember(c => c.UserCompanyGroups, m => m.MapFrom(s => Mapper.Map<List<UserCompanyGroup>, List<UserCompanyGroupDto>>(s.UserCompanyGroups != null ? s.UserCompanyGroups.ToList() : null)))
               .ForMember(c => c.UserCompanyGroupsPlain, m => m.MapFrom(s => Mapper.Map<List<UserCompanyGroup>, List<UserCompanyGroupPlainDto>>(s.UserCompanyGroups != null ? s.UserCompanyGroups.ToList() : null)));
            Mapper.CreateMap<UserCompanyGroup, UserCompanyGroupPlainDto>()
                .ForMember(c => c.CompanyGroup, s => s.MapFrom(r => r.UserGroup.Description + " " + r.Company.Name))
                .ForMember(c => c.Company, s => s.MapFrom(r => r.Company.Name));
            Mapper.CreateMap<UserCompanyGroup, UserCompanyGroupDto>()
                .ForMember(c => c.Company, m => m.MapFrom(s => Mapper.Map<Company, CompanyDto>(s.Company)))
                .ForMember(c => c.UserGroup, m => m.MapFrom(s => Mapper.Map<UserGroup, UserGroupDto>(s.UserGroup)));
            Mapper.CreateMap<Role, RoleDto>();
            Mapper.CreateMap<UserCompany, UserCompanyDto>()
                .ForMember(c => c.Company, m => m.MapFrom(s => s.Company.Name));
            Mapper.CreateMap<UserGroup, UserGroupDto>();
            Mapper.CreateMap<Category, CategoryDto>();
            Mapper.CreateMap<Payment, PaymentDto>()
               .ForMember(c => c.User, m => m.MapFrom(s => Mapper.Map<User,UserDto>(s.User)))
               .ForMember(c => c.UserId, m => m.MapFrom(s => s.UserId));
            Mapper.CreateMap<Aliquot, AliquotDto>();
            Mapper.CreateMap<Expense, ExpenseReportDto>()
                .ForMember(c => c.Category, m => m.MapFrom(s => s.Category.Description))
                .ForMember(c => c.Payment, m => m.MapFrom(s => s.Payment.Description))
                .ForMember(c => c.PaymentType, m => m.MapFrom(s => s.Aliquot.Description))
                .ForMember(c => c.Provider, m => m.MapFrom(s => s.Provider.LegalName))
                .ForMember(c => c.ProviderCuit, m => m.MapFrom(s => s.Provider.Cuit))
                .ForMember(c => c.SyncStatus, m => m.MapFrom(s => s.SyncStatus.Description))
                .ForMember(c => c.User, m => m.MapFrom(s => Mapper.Map<User,SearchUserDto>(s.User)))
                .ForMember(c => c.Total, m => m.MapFrom(s => s.Total.ToString().Replace(',', '.')))
                .ForMember(c => c.NetValue, m => m.MapFrom(s => s.NetValue.ToString().Replace(',', '.')))
                .ForMember(c => c.Company, m => m.MapFrom(s => "AL0" + s.Company.Id))
                .ForMember(c => c.UserCode, m => m.MapFrom(s => (s.User.Id_Erp != null && s.User.Id_Erp != "0" && s.User.Id_Erp != "")? s.User.Id_Erp: "N/A"))
                .ForMember(c => c.CompanyGroup, m => m.MapFrom(s => s.Group))
                .ForMember(c => c.NotTaxedConcepts, m => m.MapFrom(s => s.NotTaxedConcepts.ToString().Replace(',', '.')))
                .ForMember(c => c.IVA, m => m.MapFrom(s => s.IVA.ToString().Replace(',', '.')));
            Mapper.CreateMap<Expense, ExpenseDto>()
              .ForMember(c => c.Images, m => m.MapFrom(s => Mapper.Map<List<Image>, List<ImageDto>>(s.Images.ToList())))
              .ForMember(c => c.Logs, m => m.MapFrom(s => Mapper.Map<List<ExpenseStatusLog>, List<ExpenseStatusesLogDto>>(s.Logs.ToList())))
              .ForMember(c => c.Aliquot, m => m.MapFrom(s => Mapper.Map<Aliquot, AliquotDto>(s.Aliquot)))
              .ForMember(c => c.Category, m => m.MapFrom(s => Mapper.Map<Category, CategoryDto>(s.Category)))
              .ForMember(c => c.User, m => m.MapFrom(s => Mapper.Map<User, UserDto>(s.User)))
              .ForMember(c => c.Payment, m => m.MapFrom(s => Mapper.Map<Payment, PaymentDto>(s.Payment)))
              .ForMember(c => c.Provider, m => m.MapFrom(s => Mapper.Map<Provider, ProviderDto>(s.Provider)))
              .ForMember(c => c.SyncStatus, m => m.MapFrom(s => Mapper.Map<SyncStatus, SyncStatusDto>(s.SyncStatus)))
              .ForMember(c => c.Total, m => m.MapFrom(s => s.Total.ToString().Replace(',','.')))
              .ForMember(c => c.NetValue, m => m.MapFrom(s => s.NetValue.ToString().Replace(',', '.')))
              .ForMember(c => c.NotTaxedConcepts, m => m.MapFrom(s => s.NotTaxedConcepts.ToString().Replace(',', '.')))
              .ForMember(c => c.IVA, m => m.MapFrom(s => s.IVA.ToString().Replace(',', '.')));

            Mapper.CreateMap<ExpenseStatusLog, ExpenseStatusesLogDto>()
                .ForMember(c => c.ExpenseId, m => m.MapFrom(s => s.Expense.Id))
                .ForMember(c => c.ExpenseUploaderFullName, m => m.MapFrom(s => s.Expense.User.FirstName + " " + s.Expense.User.LastName))
                .ForMember(c => c.UserName, m => m.MapFrom(s => s.User.FirstName + " " + s.User.LastName));
            Mapper.CreateMap<Image, ImageDto>()
                .ForMember(c => c.ExpenseId, m => m.MapFrom(s => s.Expense.Id));
            Mapper.CreateMap<Provider, ProviderDto>()
                .ForMember(c => c.LegalCondition, m => m.MapFrom(s => Mapper.Map<LegalCondition, LegalConditionDto>(s.LegalCondition)))
                .ForMember(c => c.UserFullName, m => m.MapFrom(s => s.User.FirstName + " " + s.User.LastName));
            Mapper.CreateMap<Company, CompanyDto>();
            Mapper.CreateMap<RefusalReason, RefusalReasonDto>();
            Mapper.CreateMap<ApprovalReason, ApprovalReasonDto>();
            Mapper.CreateMap<Config, ConfigDto>();
            Mapper.CreateMap<SyncStatus, SyncStatusDto>();
            Mapper.CreateMap<Device, DeviceDto>();
            Mapper.CreateMap<Notification, NotificationDto>()
                .ForMember(c => c.User, m => m.UseValue(null))
                .ForMember(c => c.Platform, m => m.MapFrom(s => s.Device.DeviceType))
                .ForMember(c => c.Device, m => m.MapFrom(s => Mapper.Map<Device, DeviceDto>(s.Device)));

            Mapper.CreateMap<Notification, NotificationDataDto>();
            Mapper.CreateMap<User, SearchUserDto>()
                .ForMember(c => c.FullName, m => m.MapFrom(s => s.FirstName + " " + s.LastName));
            Mapper.CreateMap<LegalCondition, LegalConditionDto>();
            Mapper.CreateMap<Expense, ExpenseNotificationDetailDto>()
                .ForMember(c => c.Category, m => m.MapFrom(s => s.Category.Description))
                .ForMember(c => c.Provider, m => m.MapFrom(s => s.Provider.LegalName))
                .ForMember(c => c.Payment, m => m.MapFrom(s => s.Payment.Description));

            Mapper.CreateMap<ExpenseDto, ExportExpenseDto>();
            Mapper.CreateMap<SearchResultViewModel<ExpenseDto>, SearchResultViewModel<ExportExpenseDto>>();
            //.ForMember(c => c.AliquotId, m => m.MapFrom(s => s.Aliquot.Id))
            //.ForMember(c => c.CategoryId, m => m.MapFrom(s => s.Category.Id))
            //.ForMember(c => c.PaymentId, m => m.MapFrom(s => s.Payment.Id))
            //.ForMember(c => c.PaymentTypeId, m => m.MapFrom(s => s.Aliquot.Id))
            //.ForMember(c => c.ProviderId, m => m.MapFrom(s => s.Provider.Id))
            //.ForMember(c => c.UserId, m => m.MapFrom(s => s.User.Id))
            //.ForMember(c => c.PaymentReceipt, m => m.MapFrom(s => s.Receipt))
            //.ForMember(c => c.IVA, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.IVA)))
            //.ForMember(c => c.Taxed, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.NetValue)))
            //.ForMember(c => c.NotTaxed, m => m.MapFrom(s => StringExtensionMethods.ToDouble(s.NotTaxedConcepts == "0" ? s.Total : s.NotTaxedConcepts)))
            //.ForMember(c => c.SyncStatusId, m => m.MapFrom(s => s.SyncStatus.Id))
            //.ForMember(c => c.AliquotIVA, m => m.MapFrom(s => s.Aliquot.Value.ToString()));
            Mapper.CreateMap<ImportProviderDto, ProviderDto>()
                .ForMember(c => c.LegalName, m => m.MapFrom(s => s.LegalName))
                .ForMember(c => c.Cuit, m => m.MapFrom(s => int.Parse(s.Cuit)))
                .ForMember(c => c.PhoneNumber, m => m.MapFrom(s => int.Parse(s.PhoneNumber)));
            Mapper.CreateMap<Provider, SearchProviderDto>();
            Mapper.CreateMap<ExpenseListViewCriteria, StatusChangeListCriteria>();

        }
    }
}