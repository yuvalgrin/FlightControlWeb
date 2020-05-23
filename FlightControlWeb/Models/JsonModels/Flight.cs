using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Flight
    {
        public Flight() { }
        public Flight(string flightId, double longitude, double latitude,
            int passengers, string companyName, DateTime dateTime,
            bool isExternal)
        {
            this.Flight_Id = flightId;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Passengers = passengers;
            this.Company_Name = companyName;
            this.Date_Time = dateTime;
            this.Is_External = isExternal;
        }

        [JsonProperty("flight_id")]
        public string Flight_Id { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string Company_Name { get; set; }

        [JsonProperty("date_time")]
        public DateTime Date_Time { get; set; }

        [JsonProperty("is_external")]
        public bool Is_External { get; set; }

    }
}
