using System;

namespace ALZAGRO.AppRendicionGastos.Fwk.Culture {
    public interface ITimeService {

        DateTime DateTimeNow { get; }

        DateTime LocalDateTimeNow { get; }

        DateTime ToLocalTime(DateTime date);

        DateTime ToUniversalTime(DateTime date);
    }
}