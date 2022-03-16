using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportValidator : IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return request?.From != null;
        }
    }
}
