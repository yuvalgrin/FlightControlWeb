using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.Algo;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public class FlightsManager
    {
        private static FlightsManager _instance;
        public static FlightsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FlightsManager();
                return _instance;
            }
        }

        public ConcurrentDictionary<string, FlightPlan> ActiveFlightPlans { get; set; }


        public FlightsManager()
        {
            ActiveFlightPlans = new ConcurrentDictionary<string, FlightPlan>();
            InitDummies();
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
                flights = RemoteServersConnector.Instance.GetRelativeFlights(dateTime);
            else
                flights  = new List<Flight>();

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

            Flight flight = new Flight(flightId, longitude, latitude, passengers,
                companyName, dateTime, false);

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
            bool exists = ActiveFlightPlans.TryGetValue(flightId,out flightPlan);

            if (exists)
                return flightPlan;

            return RemoteServersConnector.Instance.GetRemoteFlightPlan(flightId);
        }

        /**
        * Get a flight plan by id.
        */
        public void AddFlightPlan(FlightPlan flightPlan)
        {
            ActiveFlightPlans.TryAdd(flightPlan.ToString(), flightPlan);
        }


        /* ------------DUMMY STUFF----------- */
        public void InitDummies()
        {
            //// Dummy flight!!!
            for (int i = 1; i < 2; i++)
            {
                int num = 750 + i;
                string id = "EL" + num;

                Location loc = new Location(32.704581, 35.583124, DateTime.UtcNow);
                List<Segment> ls = new List<Segment>();
                ls.Add(new Segment(32.704581 + i * 2, 35.583124 + i * 1, 20));
                ls.Add(new Segment(33.804581 + i * 3, 35.683124 + i * 2, 30));
                ls.Add(new Segment(32.904581 + i * 2, 36.783124 + i * 3, 40));
                ls.Add(new Segment(20.904581 + i * 1, 21.783124 + i * 2, 40));
                FlightPlan flpln = new FlightPlan(id, i, "Company_" + i, loc, ls);
                ActiveFlightPlans.TryAdd(flpln.Flight_Id, flpln);
            }

            int numm = 750 + 7;
            string idd = "EL" + numm;

            Location locc = new Location(38.112375, 23.879437, DateTime.UtcNow);

            List<Segment> lss = new List<Segment>();
            lss.Add(new Segment(31.922629, 31.522594, 35)); // egypt
            lss.Add(new Segment(32.426506, 34.743033, 65)); // cyprus
            lss.Add(new Segment(26.209199, 35.055211, 305)); // greece
            FlightPlan flplnn = new FlightPlan(idd, 8, "Company_" + 7, locc, lss);
            ActiveFlightPlans.TryAdd(flplnn.Flight_Id, flplnn);
        }
    }
}
