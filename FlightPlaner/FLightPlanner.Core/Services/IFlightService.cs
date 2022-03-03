using FlightPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        public Flight GetFlightWithAirports(int id);

        public void DeleteFlightById(int id); 
    }
}
