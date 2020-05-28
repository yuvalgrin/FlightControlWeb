using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public interface IRemoteServersConnector
    {
        ConcurrentDictionary<string, Server> ActiveServers { get; set; }
        ConcurrentDictionary<string, Server> RemoteFlightIdToServer { get; set; }
        ConcurrentDictionary<string, FlightPlan> RemoteFlightIdToPlan { get; set; }

        bool AddServer(Server server);
        bool DeleteServer(string id);
        ICollection<Server> GetAllServers();
        List<Flight> GetRelativeFlights(DateTime dateTime);
        FlightPlan GetRemoteFlightPlan(string flightId);
    }
}