using System;
using System.Collections.Generic;
using System.Globalization;
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
        private IFlightsManager _flightsManager;

        public FlightsController(IFlightsManager flightsManager)
        {
            this._flightsManager = flightsManager;
        }

        [HttpGet]
        public IActionResult GetFlights([FromQuery] string relative_to)
        {
            DateTime relativeTo = DateTime.Parse(relative_to,
                CultureInfo.InvariantCulture,
               DateTimeStyles.AdjustToUniversal);
            bool syncAll = Request != null ? Request.Query.ContainsKey("sync_all") : false;
            List<Flight> flights = _flightsManager.GetRelativeFlights(relativeTo, syncAll);
            return new OkObjectResult(JsonConvert.SerializeObject(flights, Formatting.Indented));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFlight(string id)
        {
            if (_flightsManager.DeleteFlightPlan(id))
                return Ok();
            return BadRequest(new Error("Could not delete flight."));
        }

    }
}
