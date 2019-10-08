namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNameInExpenseStatusChangeEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseStatusLogs", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.ExpenseStatusLogs", "UserId");
            AddForeignKey("dbo.ExpenseStatusLogs", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseStatusLogs", "UserId", "dbo.Users");
            DropIndex("dbo.ExpenseStatusLogs", new[] { "UserId" });
            DropColumn("dbo.ExpenseStatusLogs", "UserId");
        }
    }
}
