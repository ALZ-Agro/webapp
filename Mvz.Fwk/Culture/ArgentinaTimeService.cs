using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using System;

namespace ALZAGRO.AppRendicionGastos.Fwk {
    public class ArgentinaTimeService : ITimeService {

        public DateTime DateTimeNow {
            get {
                return DateTime.UtcNow;
            }
        }

        public DateTime LocalDateTimeNow {
            get {
                return DateTime.UtcNow.AddHours(-3);
            }
        }

        public DateTime ToLocalTime(DateTime date) {
            return date.AddHours(-3);
        }

        public DateTime ToUniversalTime(DateTime date) {
            return date.AddHours(3);
        }
    }
}