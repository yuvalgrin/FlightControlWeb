using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class FlightPlan
    {
        public FlightPlan() {}

        public FlightPlan(string flightId, int passengers, string companyName,
            Location initialLocation, List<Segment> segments)
        {
            this.Flight_Id = flightId;
            this.Passengers = passengers;
            this.Company_Name = companyName;
            this.Initial_Location = initialLocation;
            this.Segments = segments;
        }

        [JsonProperty("flight_id")]
        public string Flight_Id { get; set; }

        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string Company_Name { get; set; }

        [JsonProperty("initial_location")]
        public Location Initial_Location { get; set; }

        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }

    }
}
