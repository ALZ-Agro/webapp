namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    
    public partial class UserID_ERPValidationChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesGroupCompanyCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductType = c.String(),
                        ProductCode = c.String(),
                        Description = c.String(),
                        SalesGroup = c.String(),
                        CategoryDescription = c.String(),
                        IVAType = c.String(),
                        Company = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SalesGroupCompanyCategory_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Users", "Id_Erp", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Id_Erp", c => c.Long(nullable: false));
            DropTable("dbo.SalesGroupCompanyCategories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SalesGroupCompanyCategory_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
