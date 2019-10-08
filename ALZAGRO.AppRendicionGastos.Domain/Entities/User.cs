using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using Mvz.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {

    public class User : EntityBase, IUser {

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HashedPassword { get; set; }

        public string Salt { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime? LastActivityDateTime { get; set; }

        public bool IsLocked { get; set; }

        public bool ShowNotifications { get; set; }

        public ICollection<UserCompanyGroup> UserCompanyGroups { get; set; }

        /// <summary>
        ///  Código alfanumérico de máximo 5 caracteres.
        /// </summary>
        [MaxLength(5)]
        public string Id_Erp { get; set; }

    }
}