using FlighPlanner.Models;
using FlighPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlighPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private static readonly object _lock = new object();

        [Authorize]
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [Authorize]
        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
            lock (_lock)
            {
                FlightStorage.DeleteFlight(id);
                return Ok();
            }
        }

        
        [HttpPut]
        [Route("flights")]
        [Authorize]
        public IActionResult AddFlights(AddFlightRequest request)
        {
            lock (_lock)
            {
                if (!FlightStorage.IsValid(request))
                    return BadRequest();

                if (FlightStorage.Exists(request))
                    return Conflict();

                return Created("", FlightStorage.AddFlight(request));
            }
        }
    }
}
