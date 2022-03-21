using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class ToAirportCountryValidator : IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return !string.IsNullOrEmpty(request?.To?.Country);
        }
    }
}
