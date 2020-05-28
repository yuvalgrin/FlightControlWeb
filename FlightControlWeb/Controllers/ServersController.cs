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
        private IRemoteServersConnector _remoteServersConnector;

        public ServersController(IRemoteServersConnector remoteServersConnector)
        {
            this._remoteServersConnector = remoteServersConnector;
        }

        [HttpGet]
        public IActionResult GetServers()
        {
            ICollection<Server> servers = _remoteServersConnector.GetAllServers();
            return new OkObjectResult(JsonConvert.SerializeObject(servers, Formatting.Indented));
        }

        [HttpPost]
        public IActionResult AddServer([FromBody] Server server)
        {
            if(_remoteServersConnector.AddServer(server))
                return Ok();
            return BadRequest(new Error("Could not add server."));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteServer(string id)
        {
            if(_remoteServersConnector.DeleteServer(id))
                return Ok();
            return BadRequest(new Error("Could not delete server."));
        }
    }
}