using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class FlightPlan
    {
        public FlightPlan(string flightId, int passengers, string companyName,
            Location initialLocation, List<Segment> segments)
        {
            this.FlightId = flightId;
            this.Passengers = passengers;
            this.CompanyName = companyName;
            this.InitialLocation = initialLocation;
            this.Segments = segments;
        }

        [JsonProperty("flight_id")]
        public string FlightId { get; set; }

        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        public Location InitialLocation { get; set; }

        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }

    }
}
