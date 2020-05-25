using System;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models.Algo
{
    public static class LocationInterpolator
    {
        public static Location GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - flightPlan.Initial_Location.Date_Time;
            long airtimeSeconds = timeSpan.Seconds;
            long seconds = 0;
            Segment currentSeg = null;
            Segment lastSeg = null;
            foreach (Segment segment in flightPlan.Segments)
            {
                seconds += segment.Timespan_Seconds;
                if (seconds >= airtimeSeconds)
                {
                    seconds -= segment.Timespan_Seconds;
                    currentSeg = segment;
                    break;
                }
                lastSeg = segment;
            }

            float secInSegment = Math.Abs(airtimeSeconds - seconds);
            double fraction = secInSegment / (float) currentSeg.Timespan_Seconds;

            Location fromLocation = flightPlan.Initial_Location;
            if (lastSeg != null)
            {
                fromLocation = new Location(lastSeg.Latitude, lastSeg.Longitude,
                     DateTime.UtcNow);
            }

            double distMeters = calcDistance(fromLocation.Latitude, fromLocation.Longitude,
                currentSeg.Latitude, currentSeg.Longitude);


            return getIntermeditaPoint(distMeters, fraction,
                fromLocation.Latitude, fromLocation.Longitude,
                currentSeg.Latitude, currentSeg.Longitude);
        }

        private static double calcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double deg2radMultiplier = Math.PI / 180;
            lat1 = lat1 * deg2radMultiplier;
            lon1 = lon1 * deg2radMultiplier;
            lat2 = lat2 * deg2radMultiplier;
            lon2 = lon2 * deg2radMultiplier;

            double radius = 6378.137; // earth mean radius defined by WGS84
            double dlon = lon2 - lon1;
            double distance = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(dlon)) * radius;

            return (distance);
        }


        private static Location getIntermeditaPoint(double d, double f,
            double lat1, double lon1, double lat2, double lon2)
        {
            long R = 6371 * 1000;
            double angDist = d / R;
            double a1 = 1 - f;
            double a = Math.Sin(a1* angDist) / Math.Sin(angDist);
            double b = Math.Sin(f* angDist) / Math.Sin(angDist);
            double x = a * Math.Cos(lat1) * Math.Cos(lon1) +b * Math.Cos(lat2) * Math.Cos(lon2);
            double y = a * Math.Cos(lat1) * Math.Sin(lon1) +b * Math.Cos(lat2) * Math.Sin(lon2);
            double z = a * Math.Sin(lat1) + b * Math.Sin(lat2);
            double latitude = Math.Atan2(z, Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2)));
            double longtitude = Math.Atan2(y, x);

            return new Location(latitude, longtitude, new DateTime());
        }
    }
}
