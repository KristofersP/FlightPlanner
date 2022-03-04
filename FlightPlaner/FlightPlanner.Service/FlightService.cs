using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {

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

        public bool FlightExistsInStorage(AddFlightRequest request)
        {
            return Query().Any
            (f => f.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
            f.DepartureTime == request.DepartureTime &&
            f.ArrivalTime == request.ArrivalTime &&
            f.From.AirportName.ToLower().Trim() == request.From.Airport.ToLower().Trim() &&
            f.To.AirportName.ToLower().Trim() == request.To.Airport.ToLower().Trim());
        }
    }
}
