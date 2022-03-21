using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class SearchFlightValidator : ISearchValidator

    {
        public bool IsValid(SearchFlightRequest request)
        {
            if (request.From == request.To || request.From == null || request.To == null || request.DepartureDate == null)
            {
                return true;
            }
            return false;
        }
    }
}
