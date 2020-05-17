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

        public ConcurrentDictionary<string, Flight> ActiveFlights { get; set; }
        public ConcurrentDictionary<string, FlightPlan> ActiveFlightPlans { get; set; }


        public FlightsManager()
        {
            ActiveFlights = new ConcurrentDictionary<string, Flight>();
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
            foreach (Flight fl in ActiveFlights.Values)
            {
                ActiveFlightPlans.TryGetValue(fl.FlightId, out FlightPlan flightPlan);
                if (dateTime < flightPlan.InitialLocation.DateTime)
                    continue;

                if (dateTime > getLandingDatetime(flightPlan))
                    continue;

                Location currentLocation =
                    LocationInterpolator.GetLocation(flightPlan, dateTime);

                fl.Latitude = currentLocation.Latitude;
                fl.Longitude = currentLocation.Longitude;

                flights.Add(fl);
            }

            return flights;
        }

        private DateTime getLandingDatetime(FlightPlan flightPlan)
        {
            long seconds = 0;
            foreach (Segment segment in flightPlan.Segments)
            {
                seconds += segment.Seconds;
            }

            return flightPlan.InitialLocation.DateTime.AddSeconds(seconds);
        }

        /**
         * Delete a specific flight from this server.
         */
        public bool DeleteFlight(string flightId)
        {
            return ActiveFlights.TryRemove(flightId, out _);
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
            return null;
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
            // Dummy flight!!!
            for (int i=0; i < 5 ; i++)
            {
                int num = 750 + i;
                string id = "EL" + num;
                Flight fl = new Flight(id, 32.704581+i*2, 35.583124+i * 2, i, "Company_"+i,
                DateTime.UtcNow, false);
                ActiveFlights.TryAdd(fl.FlightId, fl);

                Location loc = new Location(32.704581, 35.583124, DateTime.UtcNow);
                List<Segment> ls = new List<Segment>();
                ls.Add(new Segment(32.704581 + i * 2, 35.583124 + i * 2, 20));
                ls.Add(new Segment(33.804581 + i * 2, 35.683124 + i * 2, 30));
                ls.Add(new Segment(32.904581 + i * 2, 36.783124 + i * 2, 40));
                ls.Add(new Segment(20.904581 + i * 2, 21.783124 + i * 2, 40));
                FlightPlan flpln = new FlightPlan(id, i, "Company_" + i, loc, ls);
                ActiveFlightPlans.TryAdd(flpln.FlightId, flpln);
            }

            int numm = 750 + 7;
            string idd = "EL" + numm;
            Flight fll = new Flight(idd, 20.704581, 25.583124, 7, "Company_" + 7,
            DateTime.UtcNow, false);
            ActiveFlights.TryAdd(fll.FlightId, fll);

            Location locc = new Location(20.704581, 25.583124, DateTime.UtcNow);

            List<Segment> lss = new List<Segment>();
            lss.Add(new Segment(22.704581, 25.583124, 50));
            lss.Add(new Segment(21.804581, 29.683124, 50));
            lss.Add(new Segment(22.904581, 26.783124, 50));
            FlightPlan flplnn = new FlightPlan(idd, 8, "Company_" + 7, locc, lss);
            ActiveFlightPlans.TryAdd(flplnn.FlightId, flplnn);
        }

    }
}
