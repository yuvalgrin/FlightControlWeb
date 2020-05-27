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
    public class FlightPlanController : Controller
    {
        [HttpGet("{id}")]
        public string Get(string id)
        {
            FlightPlan flightPlan = FlightsManager.Instance.GetFlightPlan(id);
            if (flightPlan == FlightPlan.NULL)
                return "{\"Error\":\"Flight plan does not exist\"}";

            return JsonConvert.SerializeObject(flightPlan, Formatting.Indented);
        }

        [HttpPost]
        public void Post([FromBody] FlightPlan flightPlan)
        {
            FlightsManager.Instance.AddFlightPlan(flightPlan);
        }
    }
}
