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
    public class FlightPlanController : Controller
    {
        private IFlightsManager _flightsManager;

        public FlightPlanController(IFlightsManager flightsManager)
        {
            this._flightsManager = flightsManager;
        }

        [HttpGet("{id}")]
        public IActionResult GetFlightPlan(string id)
        {
            FlightPlan flightPlan = _flightsManager.GetFlightPlan(id);
            if (flightPlan == FlightPlan.NULL)
                return BadRequest(new Error("Flight plan does not exist."));
            return new OkObjectResult(JsonConvert.SerializeObject(flightPlan, Formatting.Indented));
        }

        [HttpPost]
        public IActionResult AddFlightPlan([FromBody] FlightPlan flightPlan)
        {
            if (_flightsManager.AddFlightPlan(flightPlan))
                return Ok();
            return BadRequest(new Error("Flight plan could not be added."));
        }
    }
}
