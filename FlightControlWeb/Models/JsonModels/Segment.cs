using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Segment
    {
        public Segment() { }

        public Segment(double longitude, double latitude, int seconds)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Timespan_Seconds = seconds;
        }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("timespan_seconds")]
        public int Timespan_Seconds { get; set; }
    }
}
