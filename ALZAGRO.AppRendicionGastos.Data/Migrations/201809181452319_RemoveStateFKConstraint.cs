namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStateFKConstraint : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Expenses", new[] { "StateId" });
            AlterColumn("dbo.Expenses", "StateId", c => c.Long());
            CreateIndex("dbo.Expenses", "StateId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Expenses", new[] { "StateId" });
            AlterColumn("dbo.Expenses", "StateId", c => c.Long(nullable: false));
            CreateIndex("dbo.Expenses", "StateId");
        }
    }
}
