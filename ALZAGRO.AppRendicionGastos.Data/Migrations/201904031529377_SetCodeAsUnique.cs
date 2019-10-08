namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class SetCodeAsUnique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Categories", "Code", unique: true);
            CreateIndex("dbo.UserGroups", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserGroups", new[] { "Code" });
            DropIndex("dbo.Categories", new[] { "Code" });
        }
    }
}
