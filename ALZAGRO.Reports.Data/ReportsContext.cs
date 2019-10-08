using ALZAGRO.Reports.Data.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

namespace ALZAGRO.Reports.Data {

    public class ReportsContext : DbContext, Interfaces.IReportContext {

        #region Entity Sets

        public IDbSet<ReceiptHeader> SAR_CORMVH { get; set; }
        public IDbSet<ReceiptItem> SAR_CORMVI { get; set; }
        public IDbSet<Distribution> SAR_CORMVD01 { get; set; }
        public IDbSet<TaxesDetails> SAR_CORMVI07 { get; set; }

        public IDbSet<D_Concepts> SAR_CORMVI08 { get; set; }
        public IDbSet<H_Concepts> SAR_CORMVI09 { get; set; }
        public IDbSet<DistDimTesoH> SAR_CJRMVD10 { get; set; }


        public void Commit() {
            try {
                base.SaveChanges();
            }
            catch (DbEntityValidationException e) {
                var errorMessages = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors) {
                    foreach (var ve in eve.ValidationErrors) {
                        errorMessages.AppendLine((String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                                ve.PropertyName, ve.ErrorMessage)));
                    }
                }
                throw new Exception(errorMessages.ToString());
            }
            catch (Exception) {
                throw;
            }
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            


        }

        //static ReportsContext() {
        //    Database.SetInitializer(new NullDatabaseInitializer<ReportsContext>());
        //    if (!Database.CompatibleWithModel(true)) {
        //        Console.WriteLine("BREAK");
        //    }
        //}
    }
}