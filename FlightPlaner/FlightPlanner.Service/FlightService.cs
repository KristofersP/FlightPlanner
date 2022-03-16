﻿using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        protected readonly IFlightPlannerDbContext _context;
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
            _context = context;
        }

        public Flight GetFlightWithAirports(int id)
        {
           return Query()
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);
        }

        public void DeleteFlightById(int id)
        {
            var flight = GetFlightWithAirports(id);
            if(flight != null)
            {
                Delete(flight);
            }
        }

        public bool FlightExistsInStorage(AddFlightDto request)
        {
            return Query().Any
            (f => f.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
            f.DepartureTime == request.DepartureTime &&
            f.ArrivalTime == request.ArrivalTime &&
            f.From.AirportName.ToLower().Trim() == request.From.Airport.ToLower().Trim() &&
            f.To.AirportName.ToLower().Trim() == request.To.Airport.ToLower().Trim());
        }

        public List<Airport> FindAirports(string search)
        {
            search = search.ToLower().Trim();

            var fromAirport = _context.Flights.Where(f => f.From.AirportName.ToLower().Trim().Contains(search) ||
            f.From.Country.ToLower().Trim().Contains(search) ||
            f.From.City.ToLower().Trim().Contains(search)).Select(a => a.From);

            
            var toAirport = _context.Flights.Where(f => f.To.AirportName.ToLower().Trim().Contains(search) ||
            f.To.Country.ToLower().Trim().Contains(search) ||
            f.To.City.ToLower().Trim().Contains(search)).Select(a => a.To);

            return fromAirport.Concat(toAirport).ToList();
        }

        public PageResult SearchFlights(SearchFlightRequest request)
        {
            var flight = Query()
                .Include(f => f.From)
                .Include(f => f.To)
                .Where(f =>
                    f.From.AirportName.ToLower().Trim() == request.From.ToLower().Trim() &&
                    f.To.AirportName.ToLower().Trim() == request.To.ToLower().Trim() &&
                   f.DepartureTime.Substring(0, 10) == request.DepartureDate.Substring(0, 10)).ToList();

            return new PageResult(flight);
        }
    }
}
