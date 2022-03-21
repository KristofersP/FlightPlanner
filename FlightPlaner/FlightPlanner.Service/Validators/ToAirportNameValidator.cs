using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class ToAirportNameValidator : IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return !string.IsNullOrEmpty(request?.To?.Airport);
        }
    }
}
