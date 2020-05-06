using System;
using System.Collections.Concurrent;
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

        public ConcurrentDictionary<string, Server> ActiveServers { get; set; }

        public RemoteServersConnector()
        {
            ActiveServers = new ConcurrentDictionary<string, Server>();
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

        /**
         * 
         */
        public void AddServer(Server server)
        {
            ActiveServers.TryAdd(server.ServerId, server);
        }
    }
}
