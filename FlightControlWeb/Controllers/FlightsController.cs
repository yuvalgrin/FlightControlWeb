﻿using System;
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
        public IActionResult Get([FromQuery] DateTime relative_to)
        {
            bool syncAll = Request.Query.ContainsKey("sync_all");
            List<Flight> flights = FlightsManager.Instance.GetRelativeFlights(relative_to, syncAll);
            return Ok(JsonConvert.SerializeObject(flights, Formatting.Indented));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (FlightsManager.Instance.DeleteFlightPlan(id))
                return Ok();
            return BadRequest(new Error("Could not delete flight."));
        }

    }
}
