namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Payments", new[] { "UserId" });
            AlterColumn("dbo.Payments", "UserId", c => c.Long());
            CreateIndex("dbo.Payments", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payments", new[] { "UserId" });
            AlterColumn("dbo.Payments", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Payments", "UserId");
        }
    }
}
