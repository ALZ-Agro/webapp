using System;
using ALZAGRO.AppRendicionGastos.Domain.Entities;

namespace ALZAGRO.AppRendicionGastos.Data.Migrations {
    using System.Data.Entity.Migrations;

    //Enable-Migrations 
    //Add-Migration
    //Update-Database or Update-Database –Verbose
    //Re-iniciar: Update-Database -TargetMigration:0 -force
    internal sealed class Configuration : DbMigrationsConfiguration<GeneralContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GeneralContext context) {


            var creationDateTime = DateTime.Now;

            #region UserGroup
            context.UserGroup.AddOrUpdate(x => x.Id,
                new UserGroup() {
                    Id = 1,
                    Description = "Directorio",
                    Status = 1,
                    UpdatedBy = 1,
                    Code = "D",
                    UpdatedDateTime = creationDateTime
                },
                new UserGroup() {
                    Id = 2,
                    Description = "Operaciones",
                    Status = 1,
                    Code = "O",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime
                },
                new UserGroup() {
                    Id = 3,
                    Description = "Ventas",
                    Code = "V",
                    Status = 1,
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime
                },
                new UserGroup() {
                    Id = 4,
                    Description = "Servicios Informáticos",
                    Status = 0,
                    Code = "S",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime
                },
            new UserGroup() {
                Id = 5,
                Description = "Administración",
                Status = 1,
                Code = "A",
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            }
                    );
            #endregion


            #region Categories 
            context.Category.AddOrUpdate(x => x.Id,
            new Category() {
                Id = 1,
                Code = "1",
                Description = "Comida",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GHOTEL",
                ShowTo = 1

            },
            new Category() {
                Id = 2,
                Code = "2",
                Description = "Alojamiento",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GHOTEL",
                ShowTo = 1
            },
            new Category() {
                Id = 3,
                Code = "3",
                Description = "Combustible",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GTOSMO",
                ShowTo = 2
            },
            new Category() {
                Id = 4,
                Code = "4",
                Description = "Reparaciones y repuestos",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GTOSMO",
                ShowTo = 1
            },
            new Category() {
                Id = 5,
                Code = "5",
                Description = "Peajes y Estacionamientos",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GTOSMO",
                ShowTo = 1
            },
            new Category() {
                Id = 6,
                Code = "6",
                Description = "Correos y Encomiendas",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "SERVIC",
                ShowTo = 1
            },
            new Category() {
                Id = 7,
                Code = "7",
                Description = "Taxi y Bus",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "GTOSMO",
                ShowTo = 1
            },
            new Category() {
                Id = 8,
                Code = "8",
                Description = "Gastos de Telefonía",
                Status = 1,
                ShowTo = 0,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            },
            new Category() {
                Id = 9,
                Code = "9",
                Description = "Varios",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ProductType = "SERVIC"
            },
            new Category() {
                Id = 10,
                Code = "10",
                Description = "Gastos Planta Lima",
                ProductType = "GTOSMO",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                ShowTo = 0
            }
            );
            #endregion

            #region Roles 

            context.Role.AddOrUpdate(x => x.Id,
                new Role() {
                    Id = 1,
                    Description = "Administrativo",
                    Status = 1,
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime
                },
                new Role() {
                    Id = 2,
                    Description = "Usuario",
                    Status = 1,
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime
                },
            new Role() {
                Id = 3,
                Description = "Administrador del sistema",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            }
            );

            #endregion

            #region Users

            context.Users.AddOrUpdate(x => x.Id,
                new User() {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@movizen.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    HashedPassword = "870yL2plaOStGVh1yE8cZxSRbMF1Ih+kAsnrt7SAJVc=",
                    Salt = "JZMedtyVjBrJC09/dEANpA==",
                    RoleId = 1,
                    IsLocked = false,
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                 new User() {
                     Id = 2,
                     Username = "sysadmin@yopmail.com",
                     Email = "sysadmin@yopmail.com",
                     FirstName = "Admin",
                     LastName = "Sistema",
                     HashedPassword = "870yL2plaOStGVh1yE8cZxSRbMF1Ih+kAsnrt7SAJVc=",
                     Salt = "JZMedtyVjBrJC09/dEANpA==",
                     RoleId = 3,
                     IsLocked = false,
                     UpdatedBy = 1,
                     UpdatedDateTime = creationDateTime,
                     Status = 1
                 }
            );

            #endregion

            #region UserCompanyGroup

            //context.UserCompanyGroup.AddOrUpdate(x => x.Id,
            //    new UserCompanyGroup() {
            //        Id = 1,
            //        CompanyId = 3,
            //        Status = 1,
            //        UpdatedBy = 1,
            //        UpdatedDateTime = creationDateTime,
            //        UserGroupId = 2,
            //        UserId = 2
            //    });

            //#endregion

            //#region UserCompany
            //context.UserCompany.AddOrUpdate(x => x.Id,
            //new UserCompany() {
            //    Id = 1,
            //    CompanyId = 2,
            //    Status = 1,
            //    UpdatedBy = 1,
            //    UpdatedDateTime = creationDateTime,
            //    UserId = 2,
            //});
            #endregion

            #region SyncStatus

            context.SyncStatus.AddOrUpdate(x => x.Id,
                new SyncStatus() {
                    Id = 1,
                    Description = "Pendiente",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new SyncStatus() {
                    Id = 2,
                    Description = "Rechazado",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new SyncStatus() {
                    Id = 3,
                    Description = "Aprobado",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
            new SyncStatus() {
                Id = 4,
                Description = "Nuevo",
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                Status = 1
            },
            new SyncStatus() {
                Id = 5,
                Description = "Duplicado",
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                Status = 1
            },
        new SyncStatus() {
            Id = 6,
            Description = "Editando",
            UpdatedBy = 1,
            UpdatedDateTime = creationDateTime,
            Status = 1
        }

            );

            #endregion

            #region Config

            context.Config.AddOrUpdate(x => x.Id,
                new Config() {
                    Id = 1,
                    SyncDays = 60,
                    UpdatedBy = 1,
                    SyncIntervalInSeconds = 300,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                }
            );

            #endregion

            #region Companies

            context.Company.AddOrUpdate(x => x.Id,
                new Company() {
                    Id = 1,
                    Name = "Semillas",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new Company() {
                    Id = 2,
                    Name = "Nutrientes",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new Company() {
                    Id = 3,
                    Name = "Mercados",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new Company() {
                    Id = 4,
                    Name = "Solidum",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                }
            );

            #endregion

            #region Payments

            context.Payments.AddOrUpdate(x => x.Id,
                new Payment() {
                    Id = 1,
                    Description = "Efectivo",
                    UserId = null,
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                }
            );

            #endregion

            #region Aliquot

            context.Aliquot.AddOrUpdate(x => x.Id,
                new Aliquot() {
                    Id = 1,
                    Value = 21,
                    Description = "21 %",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new Aliquot() {
                    Id = 2,
                    Value = 10.5,
                    Description = "10.5 %",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                },
                new Aliquot() {
                    Id = 3,
                    Value = 0,
                    Description = "SIN IVA",
                    UpdatedBy = 1,
                    UpdatedDateTime = creationDateTime,
                    Status = 1
                }
            );

            #endregion

            #region legalCondition
            context.LegalCondition.AddOrUpdate(x => x.Id,
            new LegalCondition() {
                Id = 1,
                Description = "Responsable Inscripto",
                UpdatedBy = 1,
                Status = 1,
                UpdatedDateTime = creationDateTime
            },
            new LegalCondition() {
                Id = 2,
                Description = "Monotributista",
                UpdatedBy = 1,
                Status = 1,
                UpdatedDateTime = creationDateTime
            },
            new LegalCondition() {
                Id = 3,
                Description = "Exento",
                UpdatedBy = 1,
                Status = 1,
                UpdatedDateTime = creationDateTime
            },
            new LegalCondition() {
                Id = 4,
                Description = "Responsable No Inscripto",
                UpdatedBy = 1,
                Status = 1,
                UpdatedDateTime = creationDateTime
            },
             new LegalCondition() {
                 Id = 5,
                 Description = "Consumidor Final",
                 UpdatedBy = 1,
                 Status = 1,
                 UpdatedDateTime = creationDateTime
             },
             new LegalCondition() {
                 Id = 6,
                 Description = "Responsable Inscripto Gran Contribuyente",
                 UpdatedBy = 1,
                 Status = 1,
                 UpdatedDateTime = creationDateTime
             },
             new LegalCondition() {
                 Description = "Responsable Inscripto Factura M",
                 UpdatedBy = 1,
                 Status = 1,
                 Id = 7,
                 UpdatedDateTime = creationDateTime
             });
            #endregion



            #region Providers
            context.Provider.AddOrUpdate(x => x.Id,
                //new Provider() {
                //    Id = 1,
                //    LegalName = "Proveedor generico",
                //    ContactFullName = "ALZAGRO GENÉRICO",
                //    LegalConditionId = 1,
                //    UpdatedBy = 1,
                //    UpdatedDateTime = creationDateTime,
                //    Status = 1,
                //    Cuit = 30709878103,
                //    Email = "info@alz-agro.com.ar",
                //    PhoneNumber = 3415300806,
                //    SyncedWithERP = false,
                //},
              new Provider() {
                Id = 1,
                Cuit = 1,
                CategoryId = 1,
                ContactFullName = "Proveedor Comidas",
                LegalConditionId = 1,
                Status = 1,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Comidas",
            },
            new Provider() {
                Id = 2,
                CategoryId = 2,
                Cuit = 2,
                ContactFullName = "Proveedor Hotel",
                LegalConditionId = 1,
                Status = 1,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Hotel"
            },
            new Provider() {
                CategoryId = 7,
                Id = 3,
                ContactFullName = "Proveedor Taxi y Bus",
                LegalConditionId = 1,
                Status = 1,
                Cuit = 7,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Taxi y Bus"
            },
            new Provider() {
                CategoryId = 5,
                Id = 4,
                Cuit = 5,
                ContactFullName = "Proveedor Peajes y Estacionamientos",
                LegalConditionId = 1,
                Status = 1,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Peajes y Estacionamientos"
            },
            new Provider() {
                Id = 5,
                CategoryId = 4,
                Cuit = 4,
                ContactFullName = "Proveedor Reparaciones y Repuestos",
                LegalConditionId = 1,
                Status = 1,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Reparaciones y Repuestos"
            },
            new Provider() {
                CategoryId = 6,
                Id = 6,
                ContactFullName = "Proveedor Correos y Encomiendas",
                LegalConditionId = 1,
                Status = 1,
                Cuit = 6,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Correos y Encomiendas"
            },
            new Provider() {
                CategoryId = 9,
                Id = 7,
                Cuit = 9,
                ContactFullName = "Proveedor Gastos Varios",
                LegalConditionId = 1,
                Status = 1,
                SyncedWithERP = true,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime,
                LegalName = "Proveedor Gastos Varios"
            }
            );

            #endregion

            #region refusalreasons
            context.RefusalReason.AddOrUpdate(x => x.Id,
            new RefusalReason() {
                Id = 1,
                Description = "Error en los datos",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            },
            new RefusalReason() {
                Id = 2,
                Description = "Carga duplicada",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            }
            );
            #endregion

            #region approvalReasons
            context.ApprovalReason.AddOrUpdate(x => x.Id,
            new ApprovalReason() {
                Id = 1,
                Description = "Motivo 1",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            },
            new ApprovalReason() {
                Id = 2,
                Description = "Motivo 2",
                Status = 1,
                UpdatedBy = 1,
                UpdatedDateTime = creationDateTime
            }
            );
            #endregion




        }
    }
}