namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddExportedDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "ExportedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "ExportedDateTime");
        }
    }
}
