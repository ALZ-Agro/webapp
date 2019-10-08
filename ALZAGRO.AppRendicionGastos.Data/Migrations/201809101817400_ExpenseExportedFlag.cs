namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpenseExportedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Exported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "Exported");
        }
    }
}
