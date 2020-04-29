using System;
using System.Collections.Generic;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public class RemoteServersConnector
    {
        private static RemoteServersConnector _instance;
        public static RemoteServersConnector Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RemoteServersConnector();
                return _instance;
            }
        }

        public Dictionary<string, Server> ActiveServers { get; set; }

        public RemoteServersConnector()
        {
        }

        /**
         * Gets the relative flights for a specific date time.
         * syncAll gets the flights from remote servers as well.
         */
        public List<Flight> GetRelativeFlights(DateTime dateTime)
        {
            List<Flight> flights = new List<Flight>();
            return flights;
        }
    }
}
