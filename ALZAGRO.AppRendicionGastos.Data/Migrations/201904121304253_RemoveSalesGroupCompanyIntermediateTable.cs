namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSalesGroupCompanyIntermediateTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SalesGroupCompanyCategories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SalesGroupCompanyCategory_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
        
        public override void Down()
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
            
        }
    }
}
