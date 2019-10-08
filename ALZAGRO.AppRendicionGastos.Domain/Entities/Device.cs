using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using Mvz.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Device : EntityBase, IEntityBase {

        public string Token { get; set; }

        [ForeignKey("User")]
        public Nullable<long> UserId { get; set; }
        public User User { get; set; }

        public string DeviceType { get; set; }

        public string InstallationId { get; set; }
        public List<string> Tags { get; set; }


        public ICollection<Notification> Notifications { get; set; }

    }
}
