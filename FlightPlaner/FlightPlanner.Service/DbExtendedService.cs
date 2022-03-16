using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class DbExtendedService : DbService, IDbExtendedService
    {
        public DbExtendedService(IFlightPlannerDbContext context) : base(context)
        {

        }
        public void DeleteAll()
        {
            _context.Airports.RemoveRange(_context.Airports);
            _context.Flights.RemoveRange(_context.Flights);
            _context.SaveChanges();
        }
    }
}
