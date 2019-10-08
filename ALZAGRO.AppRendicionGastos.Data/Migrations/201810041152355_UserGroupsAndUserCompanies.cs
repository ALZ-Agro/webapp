namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupsAndUserCompanies : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.States", newName: "UserGroups");
            DropForeignKey("dbo.Expenses", "StateId", "dbo.States");
            DropIndex("dbo.Expenses", new[] { "StateId" });
            CreateTable(
                "dbo.UserCompanies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompany_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.UserId);
            
            AlterTableAnnotations(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_State_NotDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_UserGroup_NotDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.Categories", "ProductType", c => c.String());
            AddColumn("dbo.Users", "UserGroupId", c => c.Long());
            AddColumn("dbo.Expenses", "Group", c => c.String());
            AlterColumn("dbo.Expenses", "Total", c => c.Double(nullable: false));
            AlterColumn("dbo.Expenses", "NetValue", c => c.Double(nullable: false));
            AlterColumn("dbo.Expenses", "IVA", c => c.Double(nullable: false));
            AlterColumn("dbo.Expenses", "NotTaxedConcepts", c => c.Double(nullable: false));
            CreateIndex("dbo.Users", "UserGroupId");
            AddForeignKey("dbo.Users", "UserGroupId", "dbo.UserGroups", "Id");
            DropColumn("dbo.Expenses", "StateId");
            DropColumn("dbo.Expenses", "Concept");
            DropColumn("dbo.Expenses", "GrossIncome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "GrossIncome", c => c.Single());
            AddColumn("dbo.Expenses", "Concept", c => c.String(maxLength: 200));
            AddColumn("dbo.Expenses", "StateId", c => c.Long());
            DropForeignKey("dbo.Users", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.UserCompanies", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompanies", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserCompanies", new[] { "UserId" });
            DropIndex("dbo.UserCompanies", new[] { "CompanyId" });
            DropIndex("dbo.Users", new[] { "UserGroupId" });
            AlterColumn("dbo.Expenses", "NotTaxedConcepts", c => c.Single());
            AlterColumn("dbo.Expenses", "IVA", c => c.Single());
            AlterColumn("dbo.Expenses", "NetValue", c => c.Single());
            AlterColumn("dbo.Expenses", "Total", c => c.Single(nullable: false));
            DropColumn("dbo.Expenses", "Group");
            DropColumn("dbo.Users", "UserGroupId");
            DropColumn("dbo.Categories", "ProductType");
            AlterTableAnnotations(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_State_NotDeleted",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_UserGroup_NotDeleted",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            DropTable("dbo.UserCompanies",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompany_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.Expenses", "StateId");
            AddForeignKey("dbo.Expenses", "StateId", "dbo.States", "Id");
            RenameTable(name: "dbo.UserGroups", newName: "States");
        }
    }
}
