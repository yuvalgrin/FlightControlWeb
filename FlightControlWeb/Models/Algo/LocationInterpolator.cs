using System;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models.Algo
{
    public static class LocationInterpolator
    {
        public static Location GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            TimeSpan timespan = dateTime - flightPlan.Initial_Location.Date_Time;
            long airtimeSeconds = (long) timespan.TotalSeconds;
            long secondsOnSegmentsSoFar = 0;
            Segment currentSeg = null, lastSeg = null;

            foreach (Segment segment in flightPlan.Segments)
            {
                secondsOnSegmentsSoFar += segment.Timespan_Seconds;
                if (secondsOnSegmentsSoFar >= airtimeSeconds)
                {
                    currentSeg = segment;
                    secondsOnSegmentsSoFar -= segment.Timespan_Seconds;
                    break;
                }
                lastSeg = segment;
            }

            float secInSegment = Math.Abs(airtimeSeconds - secondsOnSegmentsSoFar);
            double fraction = secInSegment / currentSeg.Timespan_Seconds;

            Location fromLocation = flightPlan.Initial_Location;
            if (lastSeg != null)
                fromLocation = new Location(lastSeg.Longitude, lastSeg.Latitude,
                     DateTime.UtcNow);

            Location toLocation = new Location(currentSeg.Longitude, currentSeg.Latitude,
                     DateTime.UtcNow);

            return GetIntermediateLocation(fromLocation, toLocation, fraction);
        }

        public static Location GetIntermediateLocation(Location fromLocation, Location toLocation, double fraction)
        {
            var lon = fromLocation.Longitude + fraction * (toLocation.Longitude - fromLocation.Longitude);
            var lat = fromLocation.Latitude + fraction * (toLocation.Latitude - fromLocation.Latitude);
            return new Location(lon, lat, DateTime.UtcNow);
        }

    }
}
