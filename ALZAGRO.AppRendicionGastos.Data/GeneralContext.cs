using ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure;
using ALZAGRO.AppRendicionGastos.Data.Configurations;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using EntityFramework.DynamicFilters;
using Mvz.Fwk.Domain.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ALZAGRO.AppRendicionGastos.Data
{

    public class GeneralContext : BaseDbContext
    {

        #region Entity Sets

        public IDbSet<User> Users { get; set; }

        public IDbSet<Payment> Payments { get; set; }

        public IDbSet<Provider> Provider { get; set; }

        public IDbSet<Image> Image { get; set; }

        public IDbSet<Category> Category { get; set; }

        public IDbSet<Aliquot> Aliquot { get; set; }

        public IDbSet<Expense> Expense { get; set; }

        public IDbSet<SyncStatus> SyncStatus { get; set; }

        public IDbSet<Config> Config { get; set; }

        public IDbSet<Company> Company { get; set; }

        public IDbSet<Role> Role { get; set; }

        public IDbSet<RefusalReason> RefusalReason { get; set; }

        public IDbSet<Device> Device { get; set; }

        public IDbSet<Notification> Notification { get; set; }

        public IDbSet<LegalCondition> LegalCondition { get; set; }

        public IDbSet<ApprovalReason> ApprovalReason { get; set; }

        public IDbSet<ExpenseStatusLog> ExpenseStatusLog { get; set; }

        public IDbSet<UserCompany> UserCompany { get; set; }

        public IDbSet<UserGroup> UserGroup { get; set; }

        public IDbSet<UserCompanyGroup> UserCompanyGroup { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Filter("NotDeleted", (EntityBase d) => d.Status == 1 || d.Status == 2);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new PaymentConfiguration());
            modelBuilder.Configurations.Add(new ExpenseConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new ImageConfiguration());
            modelBuilder.Configurations.Add(new ProviderConfiguration());
            modelBuilder.Configurations.Add(new DevConfig());

            modelBuilder.Entity<Category>()
                .HasIndex(x => x.Code)
                .IsUnique();
            modelBuilder.Entity<UserGroup>()
                .HasIndex(x => x.Code)
                .IsUnique();

        }

        public override Exception ProcessException(Exception ex)
        {
            if (ex.InnerException?.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message) && ex.InnerException.InnerException.Message.Contains("FK") && ex.InnerException.InnerException.Message.Contains("INSERT"))
            {
                return new Exception("No es posible agregar el registro. Problem de FK. " + ex.InnerException.InnerException.Message);
            }

            if (ex.InnerException?.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message) && ex.InnerException.InnerException.Message.Contains("FK"))
            {
                return new Exception("No es posible eliminar el registro ya que existen otros registros relacionados al mismo.");
            }

            if (ex.InnerException?.InnerException == null) return ex;

            if (ex.InnerException.InnerException.Message.Contains("UK_Users_Email"))
            {
                return new Exception("El email ingresado ya se encuentra en uso.");
            }

            if (ex.InnerException.InnerException.Message.Contains("UK_Users_UserName"))
            {
                return new Exception("El nombre de usuario ingresado ya se encuentra en uso.");
            }

            if (ex.InnerException.InnerException.Message.Contains("IX_Code"))
            {
                return new Exception("El código ingresado ya está siendo utilizado.");
            }
            return ex;
        }
    }
}