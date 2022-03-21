using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AirportNameEqualityValidator : IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return request.From.Airport.ToLower().Trim() != request.To.Airport.ToLower().Trim();
        }
    }
}
