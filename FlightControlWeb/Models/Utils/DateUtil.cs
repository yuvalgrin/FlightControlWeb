using System;
using Newtonsoft.Json.Converters;

namespace FlightControlWeb.Models.Utils
{
    public static class DateUtil
    {
        public const string IsoDateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        /* Calculate diff in seconds between two datetime object */
        public static long CalcDiffInSec(DateTime from, DateTime to)
        {
            TimeSpan timespan = from - to;
            return (long)timespan.TotalSeconds;
        }

        /* Format the date object into iso date format */
        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString(IsoDateFormat);
        }
    }

    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
