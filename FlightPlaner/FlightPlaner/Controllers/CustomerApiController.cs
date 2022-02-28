using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors]
    public class CustomerApiController : ControllerBase
    {

        private readonly FlightPlannerDbContext _context;

        public CustomerApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {

            return Ok(FlightStorage.FindAirports(search, _context));

        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult FindFlights(SearchFlightRequest request)
        {
            if (!FlightStorage.IsValidSearchFlights(request))
                return BadRequest();

            
            return Ok(FlightStorage.SearchFlights(request, _context));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlights(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
