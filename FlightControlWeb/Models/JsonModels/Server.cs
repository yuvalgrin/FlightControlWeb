using System;
using Newtonsoft.Json;
namespace FlightControlWeb.Models.JsonModels
{
    public class Server
    {
        public Server(string serverId, string serverUrl)
        {
            this.ServerId = serverId;
            this.ServerUrl = serverUrl;
        }

        [JsonProperty("ServerId")]
        public string ServerId { get; set; }

        [JsonProperty("ServerURL")]
        public string ServerUrl { get; set; }

    }
}
