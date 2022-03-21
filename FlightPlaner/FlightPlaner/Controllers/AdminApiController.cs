using AutoMapper;
using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IEnumerable<IValidator> _validators;
        private readonly IMapper _mapper;

        public AdminApiController(IFlightService flightService, IEnumerable<IValidator> validators, IMapper mapper)
        {
            _flightService = flightService;
            _validators = validators;
            _mapper = mapper;
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
        public IActionResult AddFlights(AddFlightDto request)
        {
            lock (_lock)
            {
                if (!_validators.All(v => v.IsValid(request)))
                    return BadRequest();

                if (_flightService.FlightExistsInStorage(request))
                    return Conflict();

                var flight = _mapper.Map<Flight>(request);
               _flightService.Create(flight);

                return Created("", _mapper.Map<AddFlightDto>(flight));
            }
        }

        [HttpGet]
        [EnableCors]
        [Route("flights/{id}")]
        [Authorize]
        public IActionResult GetAdminFlights(int id)
        {
            var flight = _flightService.GetFlightWithAirports(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AddFlightDto>(flight));
        }
    }
}
