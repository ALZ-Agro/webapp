using System;
using System.Configuration;

namespace ALZAGRO.AppRendicionGastos.Fwk.Culture {
    public class TimeManager {

        public static DateTime ToLocalTime(DateTime date) {

            var localTime = ConfigurationManager.AppSettings["localTime"].ToString();

            return date.AddHours(Convert.ToInt16(localTime));
        }
    }
}
