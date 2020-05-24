using System;
namespace FlightControlWeb.Models.Utils
{
    public static class FlightIdUtil
    {
        public static string GenerateFlightId(string companyName)
        {
            Random random = new Random();
            int numId = random.Next(100,999);
            string flightId = companyName.Substring(0,3) + numId;
            return flightId;
        }
    }
}
