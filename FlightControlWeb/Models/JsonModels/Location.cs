using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Location
    {
        public Location(double longitude, double latitude, DateTime dateTime)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.DateTime = dateTime;
        }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
    }
}
