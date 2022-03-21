using FlightPlanner.Core.Dto;

namespace FlightPlanner.Core.Services
{
    public interface ISearchValidator
    {
        bool IsValid(SearchFlightRequest request);
    }
}
