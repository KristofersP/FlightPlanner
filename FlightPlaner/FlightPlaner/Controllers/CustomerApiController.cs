using AutoMapper;
using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors]
    public class CustomerApiController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly IFlightService _flightService;
        private readonly IEnumerable<ISearchValidator> _validators;
        private readonly IMapper _mapper;

        public CustomerApiController(IFlightService flightService, IEnumerable<ISearchValidator> validators, IMapper mapper)
        {
            _flightService = flightService;
            _validators = validators;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var airports = _flightService.FindAirports(search);
            var airportsDto = new List<AddAirportDto>();

            foreach (var airport in airports)
            {
                airportsDto.Add(_mapper.Map<AddAirportDto>(airport));
            }

            return Ok(airportsDto);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult FindFlights(SearchFlightRequest request)
        {
                if (_validators.All(v => v.IsValid(request)))
                    return BadRequest();

                return Ok(_flightService.SearchFlights(request));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlights(int id)
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
