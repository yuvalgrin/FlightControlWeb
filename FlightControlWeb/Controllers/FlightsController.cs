using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightControlWeb.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FlightsController : Controller
    {
        [HttpGet]
        public JsonResult Get([FromQuery] DateTime relative_to)
        {
            return new JsonResult("{\"null\":"+ relative_to + "}");
        }

        [HttpGet("&is_sync")]
        public JsonResult GetSync([FromQuery] DateTime relative_to)
        {
            return new JsonResult("{\"is_sync\":" + relative_to + "}");
        }

    }
}
