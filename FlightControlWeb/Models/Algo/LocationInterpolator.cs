using System;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models.Algo
{
    public static class LocationInterpolator
    {

        public static Location GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - flightPlan.InitialLocation.DateTime;
            long airtimeSeconds = timeSpan.Seconds;
            long seconds = 0;
            Segment currentSeg = null;
            Segment lastSeg = null;
            foreach (Segment segment in flightPlan.Segments)
            {
                seconds += segment.Seconds;
                if (seconds >= airtimeSeconds)
                {
                    currentSeg = segment;
                    break;
                }
                lastSeg = segment;
            }

            long secInSegment = currentSeg.Seconds - (airtimeSeconds - seconds);
            double fraction = secInSegment / currentSeg.Seconds;

            Location fromLocation = flightPlan.InitialLocation;
            if (lastSeg != null)
            {
                fromLocation = new Location(lastSeg.Latitude, lastSeg.Longitude,
                    new DateTime());
            }

            double distMeters = calcDistance(fromLocation.Latitude, fromLocation.Longitude,
                currentSeg.Latitude, currentSeg.Longitude);


            return getIntermeditaPoint(distMeters, fraction,
                fromLocation.Latitude, fromLocation.Longitude,
                currentSeg.Latitude, currentSeg.Longitude);
        }

        private static double calcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            long R = 6371*1000; // metres
            double latitude1 = lat1 * Math.PI / 180; // φ, λ in radians
            double latitude2 = lat2 * Math.PI / 180;
            double latitude = (lat2 - lat1) * Math.PI / 180;
            double longtitude = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(latitude / 2) * Math.Sin(latitude / 2) +
                      Math.Cos(latitude1) * Math.Cos(latitude2) *
                      Math.Sin(longtitude / 2) * Math.Sin(longtitude / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c; // in metres

            return d;
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
