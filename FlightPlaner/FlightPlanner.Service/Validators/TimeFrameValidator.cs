using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Services.Validators
{
    public class TimeFrameValidator : IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            var arrivalTime = DateTime.Parse(request.ArrivalTime);
            var departureTime = DateTime.Parse(request.DepartureTime);

            return arrivalTime > departureTime;
            
        }
    }
}
