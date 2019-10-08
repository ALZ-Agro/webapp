namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ExpenseStatusChangeLog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expenses", "RefusalReasonId", "dbo.RefusalReasons");
            DropIndex("dbo.Expenses", new[] { "RefusalReasonId" });
            CreateTable(
                "dbo.ExpenseStatusLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Change = c.String(),
                        ReasonOfChange = c.String(),
                        Notes = c.String(),
                        ExpenseId = c.Long(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ExpenseStatusLog_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Expenses", t => t.ExpenseId)
                .Index(t => t.ExpenseId);
            
            DropColumn("dbo.Expenses", "RefusalReasonId");
            DropColumn("dbo.Expenses", "RefusalText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "RefusalText", c => c.String());
            AddColumn("dbo.Expenses", "RefusalReasonId", c => c.Long());
            DropForeignKey("dbo.ExpenseStatusLogs", "ExpenseId", "dbo.Expenses");
            DropIndex("dbo.ExpenseStatusLogs", new[] { "ExpenseId" });
            DropTable("dbo.ExpenseStatusLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ExpenseStatusLog_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.Expenses", "RefusalReasonId");
            AddForeignKey("dbo.Expenses", "RefusalReasonId", "dbo.RefusalReasons", "Id");
        }
    }
}
