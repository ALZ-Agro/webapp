namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddCodes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Code", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Code");
        }
    }
}
