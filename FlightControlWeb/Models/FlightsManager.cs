using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

            // Always add local flights
            flights.AddRange(ActiveFlights.Values);

            return flights;
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
            for (int i=0; i < 4; i++)
            {
                int num = 750 + i;
                string id = "EL" + num;
                Flight fl = new Flight(id, 32.704581+i, 35.583124+i, i, "Company_"+i,
                new DateTime(), false);
                ActiveFlights.TryAdd(fl.FlightId, fl);

                Location loc = new Location(0,0, new DateTime());
                List<Segment> ls = new List<Segment>();
                ls.Add(new Segment(32.704581, 35.583124, 2));
                ls.Add(new Segment(33.804581, 35.683124, 3));
                ls.Add(new Segment(32.904581, 36.783124, 4));
                FlightPlan flpln = new FlightPlan(id, i, "Company_" + i, loc, ls);
                ActiveFlightPlans.TryAdd(flpln.FlightId, flpln);
            }

            int numm = 750 + 7;
            string idd = "EL" + numm;
            Flight fll = new Flight(idd, 20.704581, 25.583124, 7, "Company_" + 7,
            new DateTime(), false);
            ActiveFlights.TryAdd(fll.FlightId, fll);

            Location locc = new Location(20.704581, 25.583124, new DateTime());

            List<Segment> lss = new List<Segment>();
            lss.Add(new Segment(22.704581, 25.583124, 2));
            lss.Add(new Segment(23.804581, 25.683124, 3));
            lss.Add(new Segment(22.904581, 26.783124, 4));
            FlightPlan flplnn = new FlightPlan(idd, 8, "Company_" + 7, locc, lss);
            ActiveFlightPlans.TryAdd(flplnn.FlightId, flplnn);
        }

    }
}
