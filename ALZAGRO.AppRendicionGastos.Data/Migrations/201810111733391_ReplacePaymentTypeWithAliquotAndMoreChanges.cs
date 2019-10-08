namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    
    public partial class ReplacePaymentTypeWithAliquotAndMoreChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expenses", "PaymentTypeId", "dbo.PaymentTypes");
            DropIndex("dbo.Expenses", new[] { "PaymentTypeId" });
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            AddColumn("dbo.Categories", "ShowTo", c => c.Long(nullable: false));
            AddColumn("dbo.Expenses", "Receipt", c => c.String(nullable: false));
            AlterColumn("dbo.Expenses", "AliquotId", c => c.Long(nullable: false));
            CreateIndex("dbo.Expenses", "AliquotId");
            DropColumn("dbo.Expenses", "PaymentTypeId");
            DropTable("dbo.PaymentTypes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PaymentType_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentTypes",
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
                    { "DynamicFilter_PaymentType_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Expenses", "PaymentTypeId", c => c.Long(nullable: false));
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            AlterColumn("dbo.Expenses", "AliquotId", c => c.Long());
            DropColumn("dbo.Expenses", "Receipt");
            DropColumn("dbo.Categories", "ShowTo");
            CreateIndex("dbo.Expenses", "AliquotId");
            CreateIndex("dbo.Expenses", "PaymentTypeId");
            AddForeignKey("dbo.Expenses", "PaymentTypeId", "dbo.PaymentTypes", "Id");
        }
    }
}
