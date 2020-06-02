using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.Algo;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public class FlightsManager : IFlightsManager
    {
        private ConcurrentDictionary<string, FlightPlan> ActiveFlightPlans { get; set; }
        private IRemoteServersConnector _remoteServersConnector;

        public FlightsManager(IRemoteServersConnector remoteServersConnector)
        {
            this._remoteServersConnector = remoteServersConnector;
            ActiveFlightPlans = new ConcurrentDictionary<string, FlightPlan>();
        }

        /**
         * Gets the relative flights for a specific date time.
         * syncAll gets the flights from remote servers as well.
         */
        public List<Flight> GetRelativeFlights(DateTime dateTime, bool syncAll)
        {
            List<Flight> flights;

            // Add remote flights if needed
            if (syncAll)
                flights = _remoteServersConnector.GetRelativeFlights(dateTime);
            else
                flights = new List<Flight>();

            // Always add local flights TODO: UTC time
            foreach (FlightPlan flightPlan in ActiveFlightPlans.Values)
            {
                if (dateTime < flightPlan.Initial_Location.Date_Time)
                    continue;

                if (dateTime > GetLandingDatetime(flightPlan))
                    continue;

                Flight flight = GetFlightFromPlan(flightPlan, dateTime);

                flights.Add(flight);
            }

            return flights;
        }

        private DateTime GetLandingDatetime(FlightPlan flightPlan)
        {
            long seconds = 0;
            foreach (Segment segment in flightPlan.Segments)
            {
                seconds += segment.Timespan_Seconds;
            }

            DateTime initialDatetime = flightPlan.Initial_Location.Date_Time;
            return initialDatetime.AddSeconds(seconds);
        }

        /* Create a flight object from the flight plan
         * interpulate the exact location according dateTime */
        private Flight GetFlightFromPlan(FlightPlan flightPlan, DateTime dateTime)
        {
            Location currentLocation =
                LocationInterpolator.GetLocation(flightPlan, dateTime);
            string flightId = flightPlan.Flight_Id;
            double longitude = currentLocation.Longitude;
            double latitude = currentLocation.Latitude;
            int passengers = flightPlan.Passengers;
            string companyName = flightPlan.Company_Name;

            Flight flight = new Flight(flightId, longitude, latitude,
                passengers, companyName, dateTime, false);

            return flight;
        }

        /**
         * Delete a specific flight from this server.
         */
        public bool DeleteFlightPlan(string flightId)
        {
            return ActiveFlightPlans.TryRemove(flightId, out _);
        }


        /**
         * Get a flight plan by id.
         */
        public FlightPlan GetFlightPlan(string flightId)
        {
            FlightPlan flightPlan;
            if (ActiveFlightPlans.TryGetValue(flightId, out flightPlan))
                return flightPlan;

            return _remoteServersConnector.GetRemoteFlightPlan(flightId);
        }

        /**
        * Get a flight plan by id.
        */
        public bool AddFlightPlan(FlightPlan flightPlan)
        {
            return ActiveFlightPlans.TryAdd(flightPlan.Flight_Id, flightPlan);
        }

    }
}
