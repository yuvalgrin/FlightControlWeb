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
        public IActionResult Get()
        {
            ICollection<Server> servers = RemoteServersConnector.Instance.GetAllServers();
            return Ok(JsonConvert.SerializeObject(servers, Formatting.Indented));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Server server)
        {
            if(RemoteServersConnector.Instance.AddServer(server))
                return Ok();
            return BadRequest(new Error("Could not add server."));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(RemoteServersConnector.Instance.DeleteServer(id))
                return Ok();
            return BadRequest(new Error("Could not delete server."));
        }
    }
}