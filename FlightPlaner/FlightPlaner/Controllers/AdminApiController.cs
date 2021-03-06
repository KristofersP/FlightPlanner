using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly FlightPlannerDbContext _context;

        public AdminApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [EnableCors]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
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


        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
            lock (_lock)
            {
                var flight = _context.Flights.Include(f => f.From)
                    .Include(f => f.To)
                    .SingleOrDefault(f => f.Id == id);

                if (flight != null)
                {
                    _context.Flights.Remove(flight);
                    _context.SaveChanges();
                    return Ok();
                }

                return Ok();

            }
        }


        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlights(AddFlightRequest request)
        {
            lock (_lock)
            {
                if (!FlightStorage.IsValid(request))
                    return BadRequest();

                if (FlightExistsInStorage(request))
                    return Conflict();

                var flight = FlightStorage.ConvertToFlight(request);
                _context.Flights.Add(flight);
                _context.SaveChanges();

                return Created("", flight);
            }
        }

        private bool FlightExistsInStorage(AddFlightRequest request)
        {
            return _context.Flights.Any
            (f => f.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
            f.DepartureTime == request.DepartureTime &&
            f.ArrivalTime == request.ArrivalTime &&
            f.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
            f.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim());
        }
    }
}
