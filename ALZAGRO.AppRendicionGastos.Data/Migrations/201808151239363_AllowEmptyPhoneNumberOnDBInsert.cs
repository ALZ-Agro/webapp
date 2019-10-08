namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowEmptyPhoneNumberOnDBInsert : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Providers", "Cuit", c => c.Long());
            AlterColumn("dbo.Providers", "Email", c => c.String(maxLength: 200));
            AlterColumn("dbo.Providers", "PhoneNumber", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Providers", "PhoneNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.Providers", "Email", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Providers", "Cuit", c => c.Long(nullable: false));
        }
    }
}
