namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class UserGroupCompany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserGroupId", "dbo.UserGroups");
            DropIndex("dbo.Users", new[] { "UserGroupId" });
            CreateTable(
                "dbo.UserCompanyGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CompanyId = c.Long(nullable: false),
                        UserGroupId = c.Long(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompanyGroup_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.UserGroupId);
            
            DropColumn("dbo.Users", "UserGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserGroupId", c => c.Long());
            DropForeignKey("dbo.UserCompanyGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompanyGroups", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.UserCompanyGroups", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserCompanyGroups", new[] { "UserGroupId" });
            DropIndex("dbo.UserCompanyGroups", new[] { "CompanyId" });
            DropIndex("dbo.UserCompanyGroups", new[] { "UserId" });
            DropTable("dbo.UserCompanyGroups",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompanyGroup_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.Users", "UserGroupId");
            AddForeignKey("dbo.Users", "UserGroupId", "dbo.UserGroups", "Id");
        }
    }
}
