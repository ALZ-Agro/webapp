using Autofac;
using Autofac.Integration.WebApi;
using ALZAGRO.AppRendicionGastos.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Reflection;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Services;
using ALZAGRO.AppRendicionGastos.Data;
using FluentValidation;
using FluentValidation.WebApi;
using System.Reflection;
using System.Web.Http;
using ALZAGRO.Reports.Data;
using ALZAGRO.Reports.Data.Interfaces;

namespace ALZAGRO.AppRendicionGastos.WebUI.App_Start {

    public class AutofacWebapiConfig
    {

        public static IContainer container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterType<GeneralContext>()
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReportsContext>()
               .As<IReportContext>()
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReportRepository<>))
                .As(typeof(IReportRepository<>))
                 .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReportsUnitOfWork>()
               .As<IReportsUnnitOfWork>()
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Fwk.Data.Repositories.IEntityBaseRepository<>))
                   .As(typeof(Fwk.Domain.IEntityBaseRepository<>))
                   .InstancePerLifetimeScope();

            // Application Services

            builder.RegisterType<ErrorAppService>()
              .As<IErrorAppService>()
              .InstancePerLifetimeScope();

            builder.RegisterType<UserAppService>()
                .As<IUserAppService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentAppService>()
            .As<IPaymentAppService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<ExpenseAppService>()
            .As<IExpenseAppService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<CategoryAppService>()
            .As<ICategoryAppService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<DataService>()
            .As<IDataService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<NotificationAppService>()
           .As<INotificationAppService>()
           .InstancePerLifetimeScope();

            builder.RegisterType<ProviderAppService>()
            .As<IProviderAppService>()
            .InstancePerLifetimeScope();


            //App

            // Services
            builder.RegisterType<EncryptionService>()
                .As<IEncryptionService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailService>()
                .As<IEmailService>()
                .InstancePerLifetimeScope();


            builder.RegisterType<ArgentinaTimeService>()
              .As<ITimeService>()
              .InstancePerLifetimeScope();

            #region Validation      

            var assemblyName = "ALZAGRO.AppRendicionGastos.Application";
            var assembly = AssemblyHelper.GetAssemblyByName(assemblyName);
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<FluentValidationModelValidatorProvider>()
                .As<System.Web.Http.Validation.ModelValidatorProvider>();

            builder.RegisterType<AutofacValidatorFactory>()
                .As<IValidatorFactory>()
                .InstancePerLifetimeScope();

            #endregion

            container = builder.Build();
            return container;
        }
    }
}