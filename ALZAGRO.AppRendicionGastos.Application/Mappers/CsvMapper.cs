using System.Reflection;
using ALZAGRO.AppRendicionGastos.Data;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using CsvHelper.Configuration;
using Autofac.Integration.WebApi;
using Autofac;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.Mappers;
using System.Linq;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Mappings {


    public sealed class ExpenseListViewCriteriaMap : ClassMap<ExpenseListViewCriteria> {

        public ExpenseListViewCriteriaMap(long TotalPages) {
            Map(m => m.Page).Name("Página").ConvertUsing(s => (s.Page + 1) + "/" + TotalPages);
            Map(m => m.CompanyId).Name("Compañía").TypeConverter<CompanyNormalizer>();
            Map(m => m.CategoryId).Name("Categorías").TypeConverter<CategoryNormalizer>();
            Map(m => m.SyncStatusId).Name("Estado").TypeConverter<SyncStatusNormalizer>();
            Map(m => m.UserId).Name("Vendedor").TypeConverter<UserNormalizer>();
            Map(m => m.Exported).Name("Exportados").ConvertUsing(s => s.Exported == 0 ? "Todos" : (s.Exported == 1 ? "Exportado" : "Sin exportar"));
            Map(m => m.PaymentId).Name("Forma de pago").TypeConverter<PaymentNormalizer>();
            Map(m => m.AliquotId).Ignore();
            Map(m => m.StartDate).Name("Fecha desde").TypeConverter<DateNormalizer>();
            Map(m => m.EndDate).Name("Fecha hasta").TypeConverter<DateNormalizer>();
            Map(m => m.ExpenseId).Ignore();
            Map(m => m.FileName).Ignore();
            Map(m => m.OrderBy).Ignore();
            
            Map(m => m.PartialDescription).Ignore();
            Map(m => m.ProviderId).Ignore();
            Map(m => m.Size).Ignore();
           
            
            Map(m => m.UserName).Ignore();

        }
    }
    
        public sealed class StatusChangeListViewCriteriaMap : ClassMap<StatusChangeListCriteria> {

        public StatusChangeListViewCriteriaMap(long TotalPages) {
            var container = Build();
            var userRepository = container.Resolve<IEntityBaseRepository<User>>();
            Map(m => m.OrderBy).Ignore();
            Map(m => m.PartialDescription).Ignore();
            Map(m => m.Size).Ignore();
            Map(m => m.UserName).Ignore();
            Map(m => m.FileName).Ignore();
            Map(m => m.OrderBy).Ignore();
            Map(m => m.Page).Name("Página").ConvertUsing(s => (s.Page + 1) + "/" + TotalPages);
            Map(m => m.UserId).Name("Vendedor").ConvertUsing(s => s.UserId != 0 ? userRepository.GetAll().
                                                                                                Where(r => r.Id == s.UserId).
                                                                                                Select(n => n.FirstName + " " + n.LastName).
                                                                                                FirstOrDefault() : "<vacío>");
            Map(m => m.StartDate).Name("Fecha desde").TypeConverter<DateNormalizer>();
            Map(m => m.EndDate).Name("Fecha hasta").TypeConverter<DateNormalizer>();
           
        }
        private IContainer Build() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
    public sealed class ProviderListViewCriteriaMap : ClassMap<ProviderListViewCriteria> {

        public ProviderListViewCriteriaMap(long TotalPages) {
            var container = Build();
            var userRepository = container.Resolve<IEntityBaseRepository<User>>();

            Map(m => m.GetForBackOffice).Ignore();
            Map(m => m.OrderBy).Ignore();
            Map(m => m.PartialDescription).Ignore();
            Map(m => m.Size).Ignore();
            Map(m => m.UserName).Ignore();
            Map(m => m.FileName).Ignore();
            Map(m => m.OrderBy).Ignore();
            Map(m => m.Page).Name("Página").ConvertUsing(s => (s.Page + 1) + "/" + TotalPages);
            Map(m => m.UserId).Name("Vendedor").ConvertUsing(s => s.UserId != 0 ? userRepository.GetAll().
                                                                                                Where(r => r.Id == s.UserId).
                                                                                                Select(n => n.FirstName + " " + n.LastName).
                                                                                                FirstOrDefault() : "<vacío>");
            Map(m => m.Exported).Name("Exportado").ConvertUsing(s => s.ExportStatus != 0 ? (s.ExportStatus == 1 ? "Exportado" : "Sin exportar") : "Todos");
            Map(m => m.StartDate).Name("Fecha desde").TypeConverter<DateNormalizer>();
            Map(m => m.EndDate).Name("Fecha hasta").TypeConverter<DateNormalizer>();
            Map(m => m.LegalConditionId).Ignore();
        }
        private IContainer Build() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            return builder.Build();
        }
    }



    public sealed class ImportProviderMap : ClassMap<ImportProviderDto> {

        public ImportProviderMap() {
            Map(m => m.Id).Constant(0);
            Map(m => m.ContactFullName).Name("Nombre de contacto").TypeConverter<StringNormalizer>();
            Map(m => m.Cuit).Name("CUIT").TypeConverter<StringNormalizer>();
            Map(m => m.Email).Name("Correo electrónico").TypeConverter<StringNormalizer>();
            Map(m => m.LegalCondition).Name("Condición ante AFIP").TypeConverter<StringNormalizer>();
            Map(m => m.LegalName).Name("Nombre legal").TypeConverter<StringNormalizer>();
            Map(m => m.PhoneNumber).Name("Teléfono").TypeConverter<StringNormalizer>();
            Map(m => m.SyncedWithERP).Constant(true);
        }
    }


    
    public sealed class ProviderMap : ClassMap<ProviderDto> {

        public ProviderMap() {
            var container = Build();


            var legalConditionRepository = container.Resolve<Fwk.Domain.IEntityBaseRepository<LegalCondition>>();

            Map(m => m.ContactFullName).Name("Nombre de contacto").TypeConverter<StringNormalizer>();
            Map(m => m.Cuit).Name("CUIT").TypeConverter<StringNormalizer>();
            Map(m => m.Email).Name("Correo electrónico").TypeConverter<StringNormalizer>();
            Map(m => m.LegalCondition).Name("Condición ante AFIP").ConvertUsing(s => s.LegalCondition.Id != 0 ? legalConditionRepository.GetSingle(s.LegalCondition.Id).Description : "<vacío>");
            Map(m => m.LegalName).Name("Nombre legal").TypeConverter<StringNormalizer>();
            Map(m => m.PhoneNumber).Name("Teléfono").TypeConverter<StringNormalizer>();
            Map(m => m.SyncedWithERP).Ignore();
        }

        private IContainer Build() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            return builder.Build();
        }


    }

    public sealed class ExpenseReportDtoMap : ClassMap<ExpenseReportDto> {

        public ExpenseReportDtoMap() {
            Map(m => m.Category).Name("Categoría").TypeConverter<StringNormalizer>();
            Map(m => m.SyncStatus).Name("Estado").TypeConverter<StringNormalizer>();
            Map(m => m.UserCode).Name("Codigo").TypeConverter<StringNormalizer>();
            Map(m => m.User).Name("Vendedor").ConvertUsing(s => s.User.FullName);
            Map(m => m.Company).Name("Empresa").TypeConverter<StringNormalizer>();
            Map(m => m.CompanyGroup).Name("Grupo de ventas").TypeConverter<StringNormalizer>();
            Map(m => m.Date).Name("Fecha").TypeConverter<DateNormalizer>();
            Map(m => m.ProviderCuit).Name("Proveedor ID").TypeConverter<StringNormalizer>();
            Map(m => m.Provider).Name("Proveedor").TypeConverter<StringNormalizer>();
            Map(m => m.Receipt).Name("Comprobante").TypeConverter<StringNormalizer>();
            Map(m => m.Id).Name("Gasto ID").TypeConverter<StringNormalizer>();
            /// VER
            Map(m => m.ItemNumber).Name("ITEM").TypeConverter<StringNormalizer>();
            ///

            Map(m => m.PaymentType).Name("Tipo").TypeConverter<StringNormalizer>();

            ///VER 
            Map(m => m.TaxType).Name("Impuesto").TypeConverter<StringNormalizer>();
            ///
            Map(m => m.Payment).Name("Pago").TypeConverter<StringNormalizer>();
            Map(m => m.NetValue).Name("Neto SIN IVA").TypeConverter<StringNormalizer>();
            ///VER
            Map(m => m.InterfaceValue).Name("Importe Interfaz").TypeConverter<StringNormalizer>();
            ///

            Map(m => m.NotTaxedConcepts).Name("C. No Gravados").TypeConverter<StringNormalizer>();
            Map(m => m.IVA).Name("IVA").TypeConverter<StringNormalizer>();
            Map(m => m.Total).Name("Total").TypeConverter<StringNormalizer>();
            Map(m => m.Exported).Name("Exportado").ConvertUsing(s => s.Exported ? "Exportado" : "Sin exportar");
            Map(m => m.ExportedDateTime).Name("Fecha exportacion").TypeConverter<DateAndHourNormalizer>();
            Map(m => m.VehiculeMileage).Name("Kms").TypeConverter<StringNormalizer>();
            Map(m => m.Notes).Name("Notas").TypeConverter<StringNormalizer>();
        }
        
    }


    public sealed class NewProviderReportMap : ClassMap<ProviderReportDto> {

        public NewProviderReportMap() {
            var container = Build();


            var legalConditionRepository = container.Resolve<Fwk.Domain.IEntityBaseRepository<LegalCondition>>();
            Map(m => m.UserFullName).Name("Vendedor").TypeConverter<StringNormalizer>();
            Map(m => m.UpdatedDateTime).Name("Fecha").TypeConverter<DateNormalizer>();
            Map(m => m.LegalName).Name("Nombre o Razon Social").TypeConverter<StringNormalizer>();
            Map(m => m.Cuit).Name("CUIT").TypeConverter<StringNormalizer>();
            Map(m => m.LegalCondition).Name("Condición ante AFIP").ConvertUsing(s => s.LegalCondition.Id != 0 ? legalConditionRepository.GetSingle(s.LegalCondition.Id).Description : "<vacío>");
            Map(m => m.ContactFullName).Name("Nombre de contacto").TypeConverter<StringNormalizer>();
        }

        private IContainer Build() {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            return builder.Build();
        }


    }

    //public sealed class ExpenseMap : ClassMap<ExportExpenseDto> {
    //    public ExpenseMap(DateTime dateOfExportation) {
    //        var stringifiedDateOfExportation = dateOfExportation.ToString("dd-MM-yyyy");
    //        Map(m => m.CompanyId).Name("EMPRESA").TypeConverter<CompanyNormalizer>();
    //        Map(m => m.UserId).Name("VENDEDOR").TypeConverter<UserNormalizer>();
    //        Map(m => m.ProviderId).Name("CUIT PROVEEDOR").TypeConverter<ProviderNormalizer>();
    //        Map(m => m.OriginalPaymentReceiptStructure).Name("ESTRUCTURA COMPROB").Constant("3419");
    //        Map(m => m.PaymentReceipt).Name("N° COMPROB").TypeConverter<StringNormalizer>();
    //        Map(m => m.ExportedDate).Name("FECHA").Constant(stringifiedDateOfExportation);
    //        Map(m => m.PaymentId).Name("FORMA DE PAGO").TypeConverter<PaymentNormalizer>();
    //        Map(m => m.PaymentTypeId).Name("TIPO DE GASTO").TypeConverter<PaymentTypeNormalizer>();
    //        Map(m => m.UserId).Name("COMPRADOR").ConvertUsing(x => "N/A");
    //        Map(m => m.RegistrationCoefficient).Name("COEF REGISTRACION").Constant("ARS");
    //        Map(m => m.IssuanceCurrency).Name("COEF EMISION").Constant("ARS");
    //        Map(m => m.DebtCoefficient).Name("COEF DEUDA").Constant("ARS");
    //        Map(m => m.Notes).Name("NOTAS").TypeConverter<StringNormalizer>();
    //        Map(m => m.Date).Ignore();

    //        /// Second row
    //        Map(m => m.CategoryId).Name("CATEGORIA").TypeConverter<CategoryNormalizer>();
    //        Map(m => m.Group).Name("GRUPO").TypeConverter<StringNormalizer>();
    //        Map(m => m.Quantity).Name("CANTIDAD").Constant("1");
    //        Map(m => m.Taxed).Name("GRAVADO").TypeConverter<DoubleNormalizer>();
    //        Map(m => m.NotTaxed).Name("NO GRAVADO").TypeConverter<DoubleNormalizer>();
    //        Map(m => m.IVA).Name("IVA").TypeConverter<DoubleNormalizer>();
    //        Map(m => m.AliquotId).Name("ALICUOTA IVA").TypeConverter<AliquotNormalizer>();
    //    }
    //}

    public sealed class ExpenseStatusLogMap : ClassMap<ExpenseStatusesLogDto> {
        public ExpenseStatusLogMap() {
            Map(c => c.ExpenseUploaderFullName).Name("Vendedor").TypeConverter<StringNormalizer>();
            Map(c => c.UpdatedDateTime).Name("Fecha de modificación").TypeConverter<DateNormalizer>();
            Map(c => c.UserName).Name("Modificado por").TypeConverter<StringNormalizer>();
            Map(c => c.Change).Name("Cambio").ConvertUsing(s => s.Change.Split(' ')[0] + " -> " + s.Change.Split(' ')[2]);
            Map(c => c.ReasonOfChange).Name("Motivo").TypeConverter<StringNormalizer>();
            Map(c => c.Notes).Name("Nota").TypeConverter<StringNormalizer>();
        }
    }

}