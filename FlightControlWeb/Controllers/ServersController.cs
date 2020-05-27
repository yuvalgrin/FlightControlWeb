using System;
using System.Collections.Generic;
using FlightControlWeb.Models;
using FlightControlWeb.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ServersController : Controller
    {
        [HttpGet]
        public string Get()
        {
            ICollection<Server> servers = RemoteServersConnector.Instance.GetAllServers();
            return JsonConvert.SerializeObject(servers, Formatting.Indented);
        }

        [HttpPost]
        public void Post([FromBody] Server server)
        {
            RemoteServersConnector.Instance.AddServer(server);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            RemoteServersConnector.Instance.DeleteServer(id);
        }
    }
}