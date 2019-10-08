namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ShowUserThatLoadedTheProvider : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Providers", "UserId", c => c.Long());
            CreateIndex("dbo.Providers", "UserId");
            AddForeignKey("dbo.Providers", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Providers", "UserId", "dbo.Users");
            DropIndex("dbo.Providers", new[] { "UserId" });
            DropColumn("dbo.Providers", "UserId");
        }
    }
}
