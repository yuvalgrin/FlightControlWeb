using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        bool AddFlightPlan(FlightPlan flightPlan);
        bool DeleteFlightPlan(string flightId);
        FlightPlan GetFlightPlan(string flightId);
        List<Flight> GetRelativeFlights(DateTime dateTime, bool syncAll);
        void InitDummies();
    }
}