using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors]
    public class CustomerApiController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            return Ok(FlightStorage.FindAirports(search));
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult FindFlights(SearchFlightRequest request)
        {
            if (!FlightStorage.IsValidSearchFlights(request))
                return BadRequest();

            return Ok(FlightStorage.SearchFlights());
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }
    }
}
