namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
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
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Aliquot_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Category_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SyncDays = c.Long(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Config_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Role_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        FirstName = c.String(),
                        LastName = c.String(),
                        HashedPassword = c.String(nullable: false, maxLength: 200),
                        Salt = c.String(nullable: false, maxLength: 200),
                        RoleId = c.Long(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        ShowNotifications = c.Boolean(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        PaymentTypeId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        SyncStatusId = c.Long(nullable: false),
                        ProviderId = c.Long(nullable: false),
                        AliquotId = c.Long(),
                        PaymentId = c.Long(nullable: false),
                        CompanyId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        RefusalReasonId = c.Long(),
                        RefusalText = c.String(),
                        Notes = c.String(maxLength: 120),
                        Total = c.Single(nullable: false),
                        NetValue = c.Single(),
                        Date = c.DateTime(nullable: false),
                        Concept = c.String(maxLength: 200),
                        IVA = c.Single(),
                        GrossIncome = c.Single(),
                        NotTaxedConcepts = c.Single(),
                        VehiculeMileage = c.Long(),
                        Exported = c.Boolean(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Expense_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Aliquots", t => t.AliquotId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeId)
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .ForeignKey("dbo.RefusalReasons", t => t.RefusalReasonId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.SyncStatus", t => t.SyncStatusId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PaymentTypeId)
                .Index(t => t.CategoryId)
                .Index(t => t.SyncStatusId)
                .Index(t => t.ProviderId)
                .Index(t => t.AliquotId)
                .Index(t => t.PaymentId)
                .Index(t => t.CompanyId)
                .Index(t => t.StateId)
                .Index(t => t.RefusalReasonId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExpenseId = c.Long(nullable: false),
                        Path = c.String(nullable: false, maxLength: 200),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Image_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Expenses", t => t.ExpenseId)
                .Index(t => t.ExpenseId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 100),
                        UserId = c.Long(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Payment_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PaymentType_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LegalName = c.String(nullable: false, maxLength: 200),
                        Cuit = c.Long(),
                        LegalConditionId = c.Long(nullable: false),
                        Email = c.String(maxLength: 200),
                        PhoneNumber = c.Long(),
                        ContactFullName = c.String(nullable: false, maxLength: 200),
                        SyncedWithERP = c.Boolean(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Provider_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LegalConditions", t => t.LegalConditionId)
                .Index(t => t.LegalConditionId);
            
            CreateTable(
                "dbo.LegalConditions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LegalCondition_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefusalReasons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RefusalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_State_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SyncStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SyncStatus_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Expenses", "SyncStatusId", "dbo.SyncStatus");
            DropForeignKey("dbo.Expenses", "StateId", "dbo.States");
            DropForeignKey("dbo.Expenses", "RefusalReasonId", "dbo.RefusalReasons");
            DropForeignKey("dbo.Expenses", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.Providers", "LegalConditionId", "dbo.LegalConditions");
            DropForeignKey("dbo.Expenses", "PaymentTypeId", "dbo.PaymentTypes");
            DropForeignKey("dbo.Expenses", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Images", "ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.Expenses", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Expenses", "AliquotId", "dbo.Aliquots");
            DropForeignKey("dbo.Devices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Notifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Notifications", "DeviceId", "dbo.Devices");
            DropIndex("dbo.Providers", new[] { "LegalConditionId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.Images", new[] { "ExpenseId" });
            DropIndex("dbo.Expenses", new[] { "RefusalReasonId" });
            DropIndex("dbo.Expenses", new[] { "StateId" });
            DropIndex("dbo.Expenses", new[] { "CompanyId" });
            DropIndex("dbo.Expenses", new[] { "PaymentId" });
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            DropIndex("dbo.Expenses", new[] { "ProviderId" });
            DropIndex("dbo.Expenses", new[] { "SyncStatusId" });
            DropIndex("dbo.Expenses", new[] { "CategoryId" });
            DropIndex("dbo.Expenses", new[] { "PaymentTypeId" });
            DropIndex("dbo.Expenses", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Notifications", new[] { "DeviceId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "RoleId" });
            DropIndex("dbo.Devices", new[] { "UserId" });
            DropTable("dbo.SyncStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SyncStatus_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.States",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_State_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RefusalReasons",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RefusalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LegalConditions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LegalCondition_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Providers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Provider_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PaymentTypes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PaymentType_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Payments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Payment_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Images",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Image_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Expenses",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Expense_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Role_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
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
            DropTable("dbo.Configs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Config_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Companies",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Company_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Categories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Category_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Aliquots",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Aliquot_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
