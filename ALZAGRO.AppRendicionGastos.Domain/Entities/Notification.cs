using Mvz.Fwk.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALZAGRO.AppRendicionGastos.Domain.Entities {
    public class Notification : EntityBase {

        [ForeignKey("Role")]
        public long? RoleId { get; set; }

        public Role Role { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }

        public DateTime? ExpireDateTime { get; set; }

        public User User { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }
        public string IconUrl { get; set; }

        public bool Read { get; set; }

        public string OnClick { get; set; }

        public string OnAppClick { get; set; }

        public string ClickParameter { get; set; }

        [ForeignKey("Device")]
        public Nullable<long> DeviceId { get; set; }

        public Device Device { get; set; }
    }
}
