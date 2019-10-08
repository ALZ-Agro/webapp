namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroups", "Code", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserGroups", "Code");
        }
    }
}
