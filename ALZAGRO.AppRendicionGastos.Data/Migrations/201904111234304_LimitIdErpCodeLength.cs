namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitIdErpCodeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Id_Erp", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Id_Erp", c => c.String());
        }
    }
}
