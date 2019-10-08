namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowUserLastActivityDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastActivityDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastActivityDateTime");
        }
    }
}
