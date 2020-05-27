using System;
using FlightControlWeb.Models.JsonModels;
using FlightControlWeb.Models.Utils;

namespace FlightControlWeb.Models.Algo
{
    public static class LocationInterpolator
    {
        public static Location GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            long airtimeSeconds = DateUtil.CalcDiffInSec(dateTime, flightPlan.Initial_Location.Date_Time);
            long secInSegmentsSoFar = 0;
            Segment currentSeg = null, lastSeg = null;

            foreach (Segment segment in flightPlan.Segments)
            {
                secInSegmentsSoFar += segment.Timespan_Seconds;
                if (secInSegmentsSoFar >= airtimeSeconds)
                {
                    currentSeg = segment;
                    secInSegmentsSoFar -= segment.Timespan_Seconds;
                    break;
                }
                lastSeg = segment;
            }

            float secInCurrentSegment = Math.Abs(airtimeSeconds - secInSegmentsSoFar);
            double fraction = secInCurrentSegment / currentSeg.Timespan_Seconds;

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
