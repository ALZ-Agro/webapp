namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddStates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.States",
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
                    { "DynamicFilter_State_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Expenses", "StateId", c => c.Long(nullable: false));
            CreateIndex("dbo.Expenses", "StateId");
            AddForeignKey("dbo.Expenses", "StateId", "dbo.States", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "StateId", "dbo.States");
            DropIndex("dbo.Expenses", new[] { "StateId" });
            DropColumn("dbo.Expenses", "StateId");
            DropTable("dbo.States",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_State_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
