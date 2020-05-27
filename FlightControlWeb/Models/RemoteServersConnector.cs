using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Models.JsonModels;
using FlightControlWeb.Models.Utils;
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
        public ConcurrentDictionary<string, Server> RemoteFlightIdToServer { get; set; }
        public ConcurrentDictionary<string, FlightPlan> RemoteFlightIdToPlan { get; set; }


        public RemoteServersConnector()
        {
            ActiveServers = new ConcurrentDictionary<string, Server>();
            RemoteFlightIdToServer = new ConcurrentDictionary<string, Server>();
            RemoteFlightIdToPlan = new ConcurrentDictionary<string, FlightPlan>();
        }

        /**
        * syncAll gets the flights from remote servers as well.
        */
        public FlightPlan GetRemoteFlightPlan(string flightId)
        {
            FlightPlan flightPlan;
            RemoteFlightIdToPlan.TryGetValue(flightId, out flightPlan);
            if (flightPlan != null)
                return flightPlan;

            RemoteFlightIdToServer.TryGetValue(flightId, out Server server);
            string url = server.ServerUrl + "/api/FlightPlan/" + flightId;
            try
            {
                Task<string> queryResult = ExecuteAsyncGet(url);
                flightPlan = JsonConvert.DeserializeObject<FlightPlan>(queryResult.Result);
                RemoteFlightIdToPlan.TryAdd(flightId, flightPlan);
                return flightPlan;
            }
            catch (Exception)
            {
                return FlightPlan.NULL;
            }

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
                string url = server.ServerUrl + "/api/Flights?relative_to=" + DateUtil.formatDate(dateTime);
                Task<string> queryResult = ExecuteAsyncGet(url);
                try
                {
                    List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(queryResult.Result);
                    foreach (Flight remoteFlight in flights)
                    {
                        totalFlights.Add(remoteFlight);
                        RemoteFlightIdToServer.TryAdd(remoteFlight.Flight_Id, server);
                    }
                } catch (Exception)
                {
                    continue;
                }
            }

            return totalFlights;
        }

        /** Get http async request */
        public static async Task<string> ExecuteAsyncGet(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
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
        public bool DeleteServer(string id)
        {
            return ActiveServers.TryRemove(id, out _);
        }
    }
}
