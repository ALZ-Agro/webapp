namespace ALZAGRO.AppRendicionGastos.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InicialAzure : DbMigration
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
                "dbo.ApprovalReasons",
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
                    { "DynamicFilter_ApprovalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 200),
                        ProductType = c.String(),
                        ShowTo = c.Long(nullable: false),
                        Code = c.String(nullable: false, maxLength: 2),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Category_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
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
                        SyncIntervalInSeconds = c.Long(nullable: false),
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
                        LastActivityDateTime = c.DateTime(),
                        IsLocked = c.Boolean(nullable: false),
                        ShowNotifications = c.Boolean(nullable: false),
                        Id_Erp = c.String(maxLength: 5),
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
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        Code = c.String(nullable: false, maxLength: 1),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserGroup_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        AliquotId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        SyncStatusId = c.Long(nullable: false),
                        ProviderId = c.Long(nullable: false),
                        PaymentId = c.Long(nullable: false),
                        CompanyId = c.Long(nullable: false),
                        Group = c.String(),
                        Notes = c.String(maxLength: 120),
                        Receipt = c.String(nullable: false),
                        Total = c.Double(nullable: false),
                        NetValue = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IVA = c.Double(nullable: false),
                        NotTaxedConcepts = c.Double(nullable: false),
                        VehiculeMileage = c.Long(nullable: false),
                        Exported = c.Boolean(nullable: false),
                        ExportedDateTime = c.DateTime(),
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
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .ForeignKey("dbo.SyncStatus", t => t.SyncStatusId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AliquotId)
                .Index(t => t.CategoryId)
                .Index(t => t.SyncStatusId)
                .Index(t => t.ProviderId)
                .Index(t => t.PaymentId)
                .Index(t => t.CompanyId);
            
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
                "dbo.ExpenseStatusLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Change = c.String(),
                        ReasonOfChange = c.String(),
                        Notes = c.String(),
                        ExpenseId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ExpenseStatusLog_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Expenses", t => t.ExpenseId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ExpenseId)
                .Index(t => t.UserId);
            
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
                "dbo.Providers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LegalName = c.String(nullable: false, maxLength: 200),
                        Cuit = c.Long(),
                        LegalConditionId = c.Long(nullable: false),
                        Email = c.String(maxLength: 200),
                        PhoneNumber = c.Long(),
                        ContactFullName = c.String(),
                        SyncedWithERP = c.Boolean(nullable: false),
                        CategoryId = c.Long(),
                        UserId = c.Long(),
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
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.LegalConditionId)
                .Index(t => t.UserId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCompanies", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompanies", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Expenses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Expenses", "SyncStatusId", "dbo.SyncStatus");
            DropForeignKey("dbo.Expenses", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.Providers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Providers", "LegalConditionId", "dbo.LegalConditions");
            DropForeignKey("dbo.Expenses", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "UserId", "dbo.Users");
            DropForeignKey("dbo.ExpenseStatusLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.ExpenseStatusLogs", "ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.Images", "ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.Expenses", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Expenses", "AliquotId", "dbo.Aliquots");
            DropForeignKey("dbo.Devices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompanyGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompanyGroups", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.UserCompanyGroups", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Notifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Notifications", "DeviceId", "dbo.Devices");
            DropIndex("dbo.UserCompanies", new[] { "UserId" });
            DropIndex("dbo.UserCompanies", new[] { "CompanyId" });
            DropIndex("dbo.Providers", new[] { "UserId" });
            DropIndex("dbo.Providers", new[] { "LegalConditionId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.ExpenseStatusLogs", new[] { "UserId" });
            DropIndex("dbo.ExpenseStatusLogs", new[] { "ExpenseId" });
            DropIndex("dbo.Images", new[] { "ExpenseId" });
            DropIndex("dbo.Expenses", new[] { "CompanyId" });
            DropIndex("dbo.Expenses", new[] { "PaymentId" });
            DropIndex("dbo.Expenses", new[] { "ProviderId" });
            DropIndex("dbo.Expenses", new[] { "SyncStatusId" });
            DropIndex("dbo.Expenses", new[] { "CategoryId" });
            DropIndex("dbo.Expenses", new[] { "AliquotId" });
            DropIndex("dbo.Expenses", new[] { "UserId" });
            DropIndex("dbo.UserGroups", new[] { "Code" });
            DropIndex("dbo.UserCompanyGroups", new[] { "UserGroupId" });
            DropIndex("dbo.UserCompanyGroups", new[] { "CompanyId" });
            DropIndex("dbo.UserCompanyGroups", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Notifications", new[] { "DeviceId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "RoleId" });
            DropIndex("dbo.Devices", new[] { "UserId" });
            DropIndex("dbo.Categories", new[] { "Code" });
            DropTable("dbo.UserCompanies",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompany_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RefusalReasons",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RefusalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SyncStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SyncStatus_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
            DropTable("dbo.Payments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Payment_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ExpenseStatusLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ExpenseStatusLog_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
            DropTable("dbo.UserGroups",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserGroup_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.UserCompanyGroups",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserCompanyGroup_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
            DropTable("dbo.ApprovalReasons",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApprovalReason_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Aliquots",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Aliquot_NotDeleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
