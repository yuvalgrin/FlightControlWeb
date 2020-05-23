using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Location
    {
        public Location() { }
        public Location(double longitude, double latitude, DateTime dateTime)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Date_Time = dateTime;
        }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public DateTime Date_Time { get; set; }
    }
}
