namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSyncIntervalToConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configs", "SyncIntervalInSeconds", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configs", "SyncIntervalInSeconds");
        }
    }
}
