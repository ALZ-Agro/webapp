namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAliquotPKFromExpense1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            AlterColumn("dbo.Expenses", "AliquotId", c => c.Long());
            CreateIndex("dbo.Expenses", "AliquotId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            AlterColumn("dbo.Expenses", "AliquotId", c => c.Long(nullable: false));
            CreateIndex("dbo.Expenses", "AliquotId");
        }
    }
}
