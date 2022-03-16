﻿using FlightPlanner.Core.Dto;
using FlightPlanner.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetFlightWithAirports(int id);

        void DeleteFlightById(int id);

        bool FlightExistsInStorage(AddFlightDto request);

        public List<Airport> FindAirports(string search);

        public PageResult SearchFlights(SearchFlightRequest request);

    }
}