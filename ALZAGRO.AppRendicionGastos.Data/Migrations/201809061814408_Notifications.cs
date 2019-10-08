namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Token = c.String(),
                        UserId = c.Long(),
                        DeviceType = c.String(),
                        InstallationId = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Device_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(),
                        UserId = c.Long(),
                        ExpireDateTime = c.DateTime(),
                        Type = c.String(),
                        Message = c.String(),
                        Title = c.String(),
                        IconUrl = c.String(),
                        Read = c.Boolean(nullable: false),
                        OnClick = c.String(),
                        OnAppClick = c.String(),
                        ClickParameter = c.String(),
                        DeviceId = c.Long(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Notification_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.DeviceId);
            
            AddColumn("dbo.Users", "ShowNotifications", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Notifications", "DeviceId", "dbo.Devices");
            DropIndex("dbo.Notifications", new[] { "DeviceId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "RoleId" });
            DropIndex("dbo.Devices", new[] { "UserId" });
            DropColumn("dbo.Users", "ShowNotifications");
            DropTable("dbo.Notifications",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Notification_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Devices",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Device_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
