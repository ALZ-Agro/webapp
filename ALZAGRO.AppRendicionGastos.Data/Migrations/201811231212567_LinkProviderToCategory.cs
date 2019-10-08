namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LinkProviderToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Providers", "CategoryId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Providers", "CategoryId");
        }
    }
}
