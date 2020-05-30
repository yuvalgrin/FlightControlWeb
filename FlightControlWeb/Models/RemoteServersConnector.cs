using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlightControlWeb.Models.JsonModels;
using FlightControlWeb.Models.Utils;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    public class RemoteServersConnector : IRemoteServersConnector
    {
        private ConcurrentDictionary<string, Server> ActiveServers { get; set; }
        private ConcurrentDictionary<string, Server> RemoteFlightIdToServer { get; set; }
        private ConcurrentDictionary<string, FlightPlan> RemoteFlightIdToPlan { get; set; }


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
            if (server == null)
                return FlightPlan.NULL;
            try
            {
                StringBuilder url = new StringBuilder(server.ServerUrl);
                url.Append("/api/FlightPlan/");
                url.Append(flightId);
                Task<string> queryResult = ExecuteAsyncGet(url.ToString());
                flightPlan =
                    JsonConvert.DeserializeObject<FlightPlan>(queryResult.Result);
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
                StringBuilder url = new StringBuilder(server.ServerUrl);
                url.Append("/api/Flights?relative_to=");
                url.Append(DateUtil.FormatDate(dateTime));
                try
                {
                    QueryAndGetFlights(url.ToString(), server, totalFlights);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return totalFlights;
        }

        /* Query remote server and add its flights to total flights */
        private void QueryAndGetFlights(string url, Server server,
            List<Flight> totalFlights)
        {
            Task<string> queryResult = ExecuteAsyncGet(url);
            List<Flight> flights =
                JsonConvert.DeserializeObject<List<Flight>>(queryResult.Result);
            foreach (Flight remoteFlight in flights)
            {
                remoteFlight.Is_External = true;
                totalFlights.Add(remoteFlight);
                RemoteFlightIdToServer.TryAdd(remoteFlight.Flight_Id, server);
            }
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
        public bool AddServer(Server server)
        {
            return ActiveServers.TryAdd(server.ServerId, server);
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
