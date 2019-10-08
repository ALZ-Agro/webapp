namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveConfigurationsForProvider : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Providers", "ContactFullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Providers", "ContactFullName", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
