using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Flight
    {
        public Flight(string flightId, double longitude, double latitude,
            int passengers, string companyName, DateTime dateTime,
            bool isExternal)
        {
            this.FlightId = flightId;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Passengers = passengers;
            this.CompanyName = companyName;
            this.DateTime = dateTime;
            this.IsExternal = isExternal;
        }

        [JsonProperty("flight_id")]
        public string FlightId { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

    }
}
