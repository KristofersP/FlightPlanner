using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
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
        private readonly IFlightService _flightService;
        private readonly IFlightPlannerDbContext _context;

        public AdminApiController(IFlightService flightService, IFlightPlannerDbContext context)
        {
            _flightService = flightService;
            _context = context;
        }


        [HttpGet]
        [EnableCors]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = _flightService.GetFlightWithAirports(id);

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
                _flightService.DeleteFlightById(id);

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
               _flightService.Create(flight);

                return Created("", flight);
            }
        }

        private bool FlightExistsInStorage(AddFlightRequest request)
        {
            return _context.Flights.Any
            (f => f.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
            f.DepartureTime == request.DepartureTime &&
            f.ArrivalTime == request.ArrivalTime &&
            f.From.AirportName.ToLower().Trim() == request.From.Airport.ToLower().Trim() &&
            f.To.AirportName.ToLower().Trim() == request.To.Airport.ToLower().Trim());
        }
    }
}
