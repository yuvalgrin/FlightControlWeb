using System;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models.Algo
{
    public class LocationInterpolator
    {
        private DateTime dateTime;
        private Flight _flight;

        /**
         * Manages the logic for calculating the plane's specific location.
         */
        public LocationInterpolator(Flight flight, DateTime dateTime)
        {
            this._flight = flight;
            this.dateTime = dateTime;
        }

        public Location GetLocation()
        {
            return new Location(0,0,new DateTime());
        }
    }
}
