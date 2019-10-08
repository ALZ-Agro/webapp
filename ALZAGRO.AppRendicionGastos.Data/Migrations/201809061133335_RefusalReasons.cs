namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class RefusalReasons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefusalReasons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RefusalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Expenses", "RefusalReasonId", c => c.Long());
            AddColumn("dbo.Expenses", "RefusalText", c => c.String());
            CreateIndex("dbo.Expenses", "RefusalReasonId");
            AddForeignKey("dbo.Expenses", "RefusalReasonId", "dbo.RefusalReasons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "RefusalReasonId", "dbo.RefusalReasons");
            DropIndex("dbo.Expenses", new[] { "RefusalReasonId" });
            DropColumn("dbo.Expenses", "RefusalText");
            DropColumn("dbo.Expenses", "RefusalReasonId");
            DropTable("dbo.RefusalReasons",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RefusalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
