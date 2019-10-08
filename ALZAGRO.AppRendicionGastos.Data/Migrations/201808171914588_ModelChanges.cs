namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Providers", "SyncedWithERP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Providers", "SyncedWithERP");
        }
    }
}
