namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddValueToAliquot : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Taxes", newName: "Aliquots");
            RenameColumn(table: "dbo.Expenses", name: "TaxId", newName: "AliquotId");
            RenameIndex(table: "dbo.Expenses", name: "IX_TaxId", newName: "IX_AliquotId");
            AlterTableAnnotations(
                "dbo.Aliquots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        Value = c.Double(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Aliquot_NotDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Tax_NotDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.Aliquots", "Value", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Aliquots", "Value");
            AlterTableAnnotations(
                "dbo.Aliquots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        Value = c.Double(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Aliquot_NotDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Tax_NotDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            RenameIndex(table: "dbo.Expenses", name: "IX_AliquotId", newName: "IX_TaxId");
            RenameColumn(table: "dbo.Expenses", name: "AliquotId", newName: "TaxId");
            RenameTable(name: "dbo.Aliquots", newName: "Taxes");
        }
    }
}
