using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.Models.JsonModels;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        ConcurrentDictionary<string, FlightPlan> ActiveFlightPlans { get; set; }

        void AddFlightPlan(FlightPlan flightPlan);
        bool DeleteFlightPlan(string flightId);
        FlightPlan GetFlightPlan(string flightId);
        List<Flight> GetRelativeFlights(DateTime dateTime, bool syncAll);
        void InitDummies();
    }
}