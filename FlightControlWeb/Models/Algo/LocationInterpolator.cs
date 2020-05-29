using System;
using FlightControlWeb.Models.JsonModels;
using FlightControlWeb.Models.Utils;

namespace FlightControlWeb.Models.Algo
{
    public static class LocationInterpolator
    {
        public static Location GetLocation(FlightPlan flightPlan, DateTime dateTime)
        {
            long airtimeSeconds = DateUtil.CalcDiffInSec(dateTime,
                flightPlan.Initial_Location.Date_Time);
            long secInSegmentsSoFar = 0;
            Segment currentSeg = null, lastSeg = null;

            // Add all segment seconds so far
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

            // Get the fraction of segment the plane is in
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

        /* Basic linear interpolation logic */
        public static Location GetIntermediateLocation(Location fromLocation, Location toLocation, double fraction)
        {
            double lonDistance = toLocation.Longitude - fromLocation.Longitude;
            double latDistance = toLocation.Latitude - fromLocation.Latitude;

            double lon = fromLocation.Longitude + fraction * lonDistance;
            double lat = fromLocation.Latitude + fraction * latDistance;
            lon = Math.Round(lon, 6);
            lat = Math.Round(lat, 6);
            return new Location(lon, lat, DateTime.UtcNow);
        }

    }
}
