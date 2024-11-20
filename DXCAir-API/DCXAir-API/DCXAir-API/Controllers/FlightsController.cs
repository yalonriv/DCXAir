
using Microsoft.AspNetCore.Mvc;
using DCXAir_API.Models;
using Newtonsoft.Json;
using DCXAir_API.Utils;

namespace DCXAir_API.Controllers
{
    [Route("flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {

        // List that keeps the flights
        private readonly List<Flight> Flights;

        // Constructor for save the flights in the list Flights according with the 
        //information of the son markets
        public FlightsController()
        {
            // Extracts the information from the json markets
            string miJson = System.IO.File.ReadAllText("markets.json");
            Flights = JsonConvert.DeserializeObject<List<Flight>>(miJson);
        }

        private List<Flight> searchDirectFlight(string origin, string destination)
        {
            var filteredFlights = Flights.Where(f =>
            f.Origin.ToLower().Contains(origin.ToLower()) &&
            f.Destination.ToLower().Contains(destination.ToLower())).ToList();
            return filteredFlights;
        }

        private List<Flight> convertCoinOfPriceFlights(string coin, List<Flight> FlightsList)
        {
            foreach (var flight in FlightsList)
            {
                decimal price = decimal.Parse(flight.Price);
                decimal priceConverted = CoinConversorUtil.ConvertCoin(price, coin);
                flight.Price = priceConverted.ToString();
            }
            return FlightsList;
        }

        private List<Flight> searchFlightsWithStopOver(string origen, string destino)
        {
            var flightsFromOrigin = Flights.Where(vuelo => vuelo.Origin == origen).ToList(); // Buscar vuelos desde el origen

            var flightsWithStopOver = new List<Flight>();

            foreach (var flightFromOrigun in flightsFromOrigin)
            {
                // Buscar vuelos desde la escala (es decir, el destino del primer vuelo) hacia el destino final
                var stopOverFlights = Flights.Where(stopOverFlight => stopOverFlight.Origin == flightFromOrigun.Destination && stopOverFlight.Destination == destino).ToList();

                // Si hay vuelos desde la escala al destino, los agregamos a la lista de vuelos con escala
                foreach (var stopOverFlight in stopOverFlights)
                {
                    flightsWithStopOver.Add(flightFromOrigun);
                    flightsWithStopOver.Add(stopOverFlight);
                }
            }

            return flightsWithStopOver;
        }

        private List<Flight> searchFlights(string origin, string destination)
        {
            // First we search direct flightd
            var flightsDirect = searchDirectFlight(origin, destination);
            var resultFlights = new List<Flight>();
            if (flightsDirect.Count != 0)
            {
                resultFlights = flightsDirect;
            }
            // If there aren't direct flight, it's searched a flight with stopOver
            else
            {
                resultFlights = searchFlightsWithStopOver(origin, destination);
            }
            return resultFlights;
        }

        [HttpGet]
        [Route("getOrigins")]
        public IActionResult GetOrigins()
        {
            HashSet<string> origins = new HashSet<string>();
            foreach (var flight in Flights)
            {
                origins.Add(flight.Origin);
            }

            return Ok(new List<string>(origins));

        }

        [HttpGet]
        [Route("getDestinations")]
        public IActionResult GetDestinations()
        {
            HashSet<string> destinations = new HashSet<string>();
            foreach (var flight in Flights)
            {
                destinations.Add(flight.Destination);                                                                              
            }

            return Ok(new List<string>(destinations));

        }

        [HttpPost("filterByOneWay")]
        public IActionResult GetFlightsByOrigin([FromBody] FlightFilterDTO filter)
        {
            if (filter == null || string.IsNullOrEmpty(filter.Origin) || string.IsNullOrEmpty(filter.Destination))
            {
                return BadRequest("Debe proporcionar un origen válido.");
            }

            // First we search direct flightd
            var filteredOneWayFlights = searchFlights(filter.Origin, filter.Destination);

            if (filteredOneWayFlights.Count == 0)
            {
                return NotFound("There is no flights with the search criterial.");
            }
            convertCoinOfPriceFlights(filter.CoinToConvert, filteredOneWayFlights);
            return Ok(filteredOneWayFlights);
        }

        [HttpPost("filterByRoundTrip")]
        public IActionResult GetFlightsByRoundTrip([FromBody] FlightFilterDTO filter)
        {
            if (filter == null || string.IsNullOrEmpty(filter.Origin) || string.IsNullOrEmpty(filter.Destination))
            {
                return BadRequest("Debe proporcionar un origen válido.");
            }

            var filteredOneWayFlights = new List<Flight>();
            var filteredFlightsBack = new List<Flight>();
            // Flighs one way
            filteredOneWayFlights = searchFlights(filter.Origin, filter.Destination);

            // Flighs one way
            filteredFlightsBack = searchFlights(filter.Destination, filter.Origin);

            if (filteredOneWayFlights.Count == 0)
            {
                return NotFound("There is no flights from the origin.");
            }

            if (filteredFlightsBack.Count == 0)
            {
                return NotFound("There is no flights from the destination.");
            }

            var combinedFlights = new List<Flight>();
            combinedFlights.AddRange(filteredOneWayFlights);
            combinedFlights.AddRange(filteredFlightsBack);
            convertCoinOfPriceFlights(filter.CoinToConvert, combinedFlights);
            return Ok(combinedFlights);
        }
    }


} 



