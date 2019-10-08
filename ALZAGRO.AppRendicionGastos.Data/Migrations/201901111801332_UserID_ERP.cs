namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserID_ERP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Id_Erp", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Id_Erp");
        }
    }
}
