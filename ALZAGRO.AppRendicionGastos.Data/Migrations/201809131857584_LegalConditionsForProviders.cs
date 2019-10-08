namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class LegalConditionsForProviders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Providers", "PaymentTypeId", "dbo.PaymentTypes");
            //DropIndex("dbo.Providers", new[] { "PaymentTypeId" });
            //CreateTable(
            //    "dbo.LegalConditions",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Description = c.String(),
            //            UpdatedDateTime = c.DateTime(nullable: false),
            //            UpdatedBy = c.Long(nullable: false),
            //            Status = c.Int(nullable: false),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_LegalCondition_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);

            //AddColumn("dbo.Providers", "LegalConditionId", c => c.Long(nullable: false));
            //CreateIndex("dbo.Providers", "LegalConditionId");
            //AddForeignKey("dbo.Providers", "LegalConditionId", "dbo.LegalConditions", "Id");
            //DropColumn("dbo.Providers", "PaymentTypeId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Providers", "PaymentTypeId", c => c.Long(nullable: false));
            //DropForeignKey("dbo.Providers", "LegalConditionId", "dbo.LegalConditions");
            //DropIndex("dbo.Providers", new[] { "LegalConditionId" });
            //DropColumn("dbo.Providers", "LegalConditionId");
            //DropTable("dbo.LegalConditions",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_LegalCondition_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //CreateIndex("dbo.Providers", "PaymentTypeId");
            AddForeignKey("dbo.Providers", "PaymentTypeId", "dbo.PaymentTypes", "Id");
        }
    }
}
