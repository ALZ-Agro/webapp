using System;
using System.Globalization;
using System.Threading;

namespace ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        public const string Separator = ":";
        public const string HoursSymbol = "hs.";
        public const string DateTimeFormat = "HH:mm";

        public static string[] ValidIntervals = new string[] { "00", "30" };

        #region Format

        public static string SetCurrentTime(this DateTime date)
        {

            return new DateTime(date.Year,
                                date.Month,
                                date.Day,
                                date.Hour,
                                date.Minute,
                                date.Second).ToLocalTime().ToString();
        }
        public static string SetStartTime(this DateTime date)
        {

            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).ToString();
        }
        public static string SetEndTime(this DateTime date)
        {

            return new DateTime(date.Year, date.Month, date.Day, 12, 0, 0).ToString();
        }

        public static string ToCompleteDateTimeFormat(this DateTime date)
        {
            return date.ToString(Constants.CompleteDateTimeFormat);
        }

        //public static string ToDateTimeStringFormat(this DateTime? date) {
        //    if (date.HasValue) {

        //        return TimeManager.ToArgentinaTime(date.Value).ToString(Constants.DateTimeDefaultFormat);
        //    }
        //    return string.Empty;
        //}

        //public static string ToDateTimeStringFormat(this DateTime date) {
        //    return TimeManager.ToArgentinaTime(date).ToString(Constants.DateTimeDefaultFormat);
        //}

        //public static string TotDateStringFormat(this DateTime? date) {
        //    if (date.HasValue) {
        //        return TimeManager.ToArgentinaTime(date.Value).ToString(Constants.DateDefaultFormart);
        //    }
        //    return string.Empty;
        //}

        //public static string TotDateStringFormat(this DateTime date) {
        //    return TimeManager.ToArgentinaTime(date).ToString(Constants.DateDefaultFormart);
        //}

        public static string ShortDate(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }


        #endregion

        #region DataRange

        public static bool IsValidDate(this DateTime dateTime, int minValue, int maxValue)
        {
            var minHours = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, minValue, 0, 0);
            var maxHours = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, maxValue, 0, 0);

            return (!(minHours > dateTime || maxHours < dateTime));
        }

        public static String ToCustomLongDateString(this DateTime time)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            return time.ToLongDateString();
        }

        //public static string GetFormattedValue(int value) {
        //    return value.ToString() + ":00";
        //}

        #endregion
    }
}
