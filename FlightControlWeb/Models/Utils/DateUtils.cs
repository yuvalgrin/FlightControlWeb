using System;
namespace FlightControlWeb.Models.Utils
{
    /**
    * Serve's as a static class for utils.
    */
    public static class DateUtils
    {

        public static long CalcDiffInSec(DateTime from, DateTime to)
        {
            long result = to.Second - from.Second;
            return result;
        }
    }
}
