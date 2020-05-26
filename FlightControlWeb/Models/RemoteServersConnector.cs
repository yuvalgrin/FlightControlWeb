using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Models.JsonModels;
using Newtonsoft.Json;

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
         * syncAll gets the flights from remote servers as well.
         */
        public List<Flight> GetRelativeFlights(DateTime dateTime)
        {
            List<Flight> totalFlights = new List<Flight>();
            // Aggregate all remote flights
            foreach (Server server in ActiveServers.Values)
            {
                string url = server.ServerUrl + "/api/Flights?relative_to=" + dateTime.ToString();
                List<Flight> flights = HttpGetFlights(url).Result;
                totalFlights.AddRange(flights);
            }

            return totalFlights;
        }

        /** Get flight from another server with http get */
        private async Task<List<Flight>> HttpGetFlights(string url)
        {
            List<Flight> flights = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(2);
                var response = await client.GetAsync(url);
                var data = response.Content.ReadAsStringAsync().Result;
                flights = JsonConvert.DeserializeObject<List<Flight>>(data);
            }

            return flights;
        }

        /** Adds a new server */
        public void AddServer(Server server)
        {
            ActiveServers.TryAdd(server.ServerId, server);
        }

        /** Gets the entire active servers */
        public ICollection<Server> GetAllServers()
        {
            return ActiveServers.Values;
        }

        /** Delete a selected server */
        public void DeleteServer(string id)
        {
            return ActiveServers.TryRemove(id, out _);
        }
    }
}
