using System;
using System.Text;

namespace FlightControlWeb.Models.Utils
{
    public static class FlightIdUtil
    {
        static char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /* Generate random flight id consists of 3 random Upper case chars,
         * and 3 random numbers */
        public static string GenerateFlightId(string companyName)
        {
            Random random = new Random();
            int numId = random.Next(100,999);
            StringBuilder sb = new StringBuilder();
            for (int i=0; i <= 2; i++)
            {
                int charIndex = random.Next(0, Chars.Length - 1);
                sb.Append(Chars[charIndex]);
            }

            string flightId = sb.ToString() + numId;
            return flightId;
        }
    }
}
