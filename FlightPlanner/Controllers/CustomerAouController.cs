using FlighPlanner.Models;
using FlighPlanner.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlighPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerAouController : ControllerBase
    {
        private static readonly object _lock = new object();

        [HttpGet]
        [EnableCors]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var airports = FlightStorage.FindAirports(search);
            return Ok(airports);
        }

        [HttpPost]
        [EnableCors]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            lock(_lock)
            {
                if (FlightStorage.IsValidSearchFlights(request))
                {
                    return Ok(FlightStorage.SearchFlights());
                }

                return BadRequest();
            }
            
        }

        [HttpGet]
        [EnableCors]
        [Route("flights/{id}")]
        public IActionResult FindFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
