using FlighPlanner.Models;

namespace FlighPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;
        

        public static Flight AddFlight(AddFlightRequest request)
        {
            var flight = new Flight
            {
                From = request.From,
                To = request.To,
                Carrier = request.Carrier,
                ArrivalTime = request.ArrivalTime,
                DepartureTime = request.DepartureTime,
                Id = ++_id
            };
           
                _flights.Add(flight);
            
            return flight;
        }

        public static Flight ConvertToFlight(AddFlightRequest request)
        {
            var flight = new Flight
            {
                From = request.From,
                To = request.To,
                Carrier = request.Carrier,
                ArrivalTime = request.ArrivalTime,
                DepartureTime = request.DepartureTime,
            };

            return flight;
        }

        public static Flight GetFlight(int id)
        {
            return _flights.SingleOrDefault(flight => flight.Id == id);
        }

        public static bool IsValidSearchFlights(SearchFlightRequest request)
        {
            if(request.From == request.To || request.From == null || request.To == null || request.DepartureDate == null)
            {
                return false;
            }
            return true;
           
        }

        public static PageResult SearchFlights()
        {
            return new PageResult(_flights);
        }

        public static void DeleteFlight(int id)
        {
            var flight = GetFlight(id);
            
                if (flight != null)
                {
                    _flights.Remove(flight);
                }
            
            
        }

        public static List<Airport> FindAirports(string search)
        {
            search = search.ToLower().Trim();

            var fromAirport = _flights.Where(f => f.From.AirportName.ToLower().Trim().Contains(search) || 
            f.From.Country.ToLower().Trim().Contains(search) || 
            f.From.City.ToLower().Trim().Contains(search)).Select(a => a.From).ToList();

            var toAirport = _flights.Where(f => f.To.AirportName.ToLower().Trim().Contains(search) ||
            f.To.Country.ToLower().Trim().Contains(search) ||
            f.To.City.ToLower().Trim().Contains(search)).Select(a => a.To).ToList();

            return fromAirport.Concat(toAirport).ToList();
        }

        public static void ClearFlights()
        {
            _flights.Clear();
            _id = 0;
        }

        public static bool Exists(AddFlightRequest request)
        {
            
                return _flights.Any(f => f.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() && f.DepartureTime == request.DepartureTime &&
            f.ArrivalTime == request.ArrivalTime && f.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
            f.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim());
            
        }

        public static bool IsValid(AddFlightRequest request)
        {
           if(request == null)
            {
                return false;
            }

           if(string.IsNullOrEmpty(request.Carrier) || string.IsNullOrEmpty(request.ArrivalTime) || string.IsNullOrEmpty(request.DepartureTime))
            {
                return false;
            }

           if(request.From == null || request.To == null)
            {
                return false;
            }

           if(string.IsNullOrEmpty(request.From.AirportName) || string.IsNullOrEmpty(request.From.City) || string.IsNullOrEmpty(request.From.Country))
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.From.AirportName) || string.IsNullOrEmpty(request.From.City) || string.IsNullOrEmpty(request.From.Country))
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.To.AirportName) || string.IsNullOrEmpty(request.To.City) || string.IsNullOrEmpty(request.To.Country))
            {
                return false;
            }

            if(request.From.Country.ToLower().Trim() == request.To.Country.ToLower().Trim() && 
                request.From.City.ToLower().Trim() == request.To.City.ToLower().Trim() && 
                request.From.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim())
            {
                return false;
            }

            var arrivalTime = DateTime.Parse(request.ArrivalTime);
            var departureTime = DateTime.Parse(request.DepartureTime);

            if(arrivalTime <= departureTime)
            {
                return false;
            }

            return true;
        }
    }
}
