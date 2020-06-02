using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public interface IRemoteServersConnector
    {
        bool AddServer(Server server);
        bool DeleteServer(string id);
        ICollection<Server> GetAllServers();
        List<Flight> GetRelativeFlights(DateTime dateTime);
        FlightPlan GetRemoteFlightPlan(string flightId);
    }
}