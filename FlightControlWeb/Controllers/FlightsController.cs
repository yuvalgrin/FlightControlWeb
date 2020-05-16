using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using FlightControlWeb.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FlightsController : Controller
    {
        [HttpGet]
        public string Get([FromQuery] DateTime relativeTo, [FromQuery] bool syncAll)
        {
            List<Flight> flights = FlightsManager.Instance.GetRelativeFlights(relativeTo, syncAll);
            return JsonConvert.SerializeObject(flights, Formatting.Indented);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            FlightsManager.Instance.DeleteFlight(id);
        }

    }
}
