using System;
namespace FlightControlWeb.Models.Utils
{
    /**
    * Serve's as a static class for utils.
    */
    public static class DateUtil
    {
        public const string format = "yyyy-MM-ddTHH:mm:ssZ";

        public static long CalcDiffInSec(DateTime from, DateTime to)
        {
            TimeSpan timespan = from - to;
            return (long)timespan.TotalSeconds;
        }

        public static string formatDate(DateTime dateTime)
        {
            return dateTime.ToString(format);
        }
    }
}
