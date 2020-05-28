using System;
using Newtonsoft.Json;

namespace FlightControlWeb.Models.JsonModels
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string message)
        {
            this.Message = message;
        }

        [JsonProperty("error")]
        public string Message { get; set; }
    }
}
