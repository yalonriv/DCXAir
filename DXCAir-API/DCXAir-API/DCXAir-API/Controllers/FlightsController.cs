
using Microsoft.AspNetCore.Mvc;
using DCXAir_API.Models;
using Newtonsoft.Json;
using DCXAir_API.Utils;
using Microsoft.Extensions.Logging;

namespace DCXAir_API.Controllers
{
    /// <summary>
    ///  Clase de control que determina los métodos y servicios que permiten 
    ///  realizar la administración de los vuelos
    /// </summary>
    [Route("flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {

        // List de vuelos
        public List<Flight> Flights { get; set; }
        private readonly ILogger<FlightsController> _logger;

        /// <summary>
        ///  Método que se encarga de dar a la lista: Flights la información
        ///  que se obtenga del json: markets.json
        /// </summary>
        public FlightsController(ILogger<FlightsController> logger = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            string miJson = System.IO.File.ReadAllText("markets.json");
            Flights = string.IsNullOrEmpty(miJson)
                    ? new List<Flight>()
                    : JsonConvert.DeserializeObject<List<Flight>>(miJson);
        }

        /// <summary>
        ///  Método que se encarga de consultar un vuelo directo
        /// </summary>
        private List<Flight> searchDirectFlight(string origin, string destination)
        {
            _logger.LogInformation("Inicio método: searchDirectFlight");
            var filteredFlights = Flights.Where(f =>
            f.Origin.ToLower().Contains(origin.ToLower()) &&
            f.Destination.ToLower().Contains(destination.ToLower())).ToList();
            _logger.LogInformation("Retornando resultado del método:searchDirectFlight");
            return filteredFlights;
        }
        /// <summary>
        ///  Método que se encarga de invocar al método ConvertCoin del utilitario:
        ///  CoinConversorUtil, para convertir los precios de los vuelos al tipo 
        ///  de moneda requerida por el filtro
        /// </summary>
        private List<Flight> convertCoinOfPriceFlights(string coin, List<Flight> FlightsList)
        {
            _logger.LogInformation("Inicio método: convertCoinOfPriceFlights");
            foreach (var flight in FlightsList)
            {
                decimal price = decimal.Parse(flight.Price);
                decimal priceConverted = CoinConversorUtil.ConvertCoin(price, coin);
                flight.Price = priceConverted.ToString();
            }
            _logger.LogInformation("Retornando resultado del método: convertCoinOfPriceFlights");
            return FlightsList;
        }

        /// <summary>
        ///  Método que se encarga de consultar los vuelos con escala, verificando que 
        ///  desde el primer origen, exista otro origen para llegar al destino 
        ///  requerido.
        /// </summary>
        private List<Flight> searchFlightsWithStopOver(string origen, string destino)
        {
            _logger.LogInformation("Inicio método: searchFlightsWithStopOver");
            var flightsFromOrigin = Flights.Where(vuelo => vuelo.Origin == origen).ToList(); // Buscar vuelos desde el origen

            var flightsWithStopOver = new List<Flight>();

            foreach (var flightFromOrigun in flightsFromOrigin)
            {
                // Se buscan los vuelos desde la escala (es decir, el destino del primer vuelo) hacia el destino final
                var stopOverFlights = Flights.Where(stopOverFlight => stopOverFlight.Origin == flightFromOrigun.Destination && stopOverFlight.Destination == destino).ToList();

                // Si hay vuelos desde la escala al destino, los agregamos a la lista de vuelos con escala
                foreach (var stopOverFlight in stopOverFlights)
                {
                    flightsWithStopOver.Add(flightFromOrigun);
                    flightsWithStopOver.Add(stopOverFlight);
                }
            }
            _logger.LogInformation("Retornando resultado del método: searchFlightsWithStopOver");
            return flightsWithStopOver;
        }

        // <summary>
        ///  Método que se encarga de consultar los vuelos, verificando que, si no se 
        ///  encuentra un vuelo directo con la invocación al método: searchDirectFlight
        ///  entonces se busquen vuelos con escala invocando al método: searchDirectFlight
        /// </summary>
        private List<Flight> searchFlights(string origin, string destination)
        {
            _logger.LogInformation("Inicio método: searchFlights");
            var flightsDirect = searchDirectFlight(origin, destination);
            var resultFlights = new List<Flight>();
            if (flightsDirect.Count != 0)
            {
                resultFlights = flightsDirect;
            } else
            {
                resultFlights = searchFlightsWithStopOver(origin, destination);
            }
            _logger.LogInformation("Retornando resultado del método: searchFlights");
            return resultFlights;
        }

        // <summary>
        ///  Servicio que se encarga de consultar los origenes disponibles
        /// </summary>
        [HttpGet]
        [Route("getOrigins")]
        public IActionResult GetOrigins()
        {
            _logger.LogInformation("Inicio método: GetOrigins");
            HashSet<string> origins = new HashSet<string>();
            foreach (var flight in Flights)
            {
                origins.Add(flight.Origin);
            }
            _logger.LogInformation("Retornando resultado del método: GetOrigins");
            return Ok(new List<string>(origins));

        }

        // <summary>
        ///  Servicio que se encarga de consultar los destinos disponibles
        /// </summary>
        [HttpGet]
        [Route("getDestinations")]
        public IActionResult GetDestinations()
        {
            _logger.LogInformation("Inicio método: GetDestinations");
            HashSet<string> destinations = new HashSet<string>();
            foreach (var flight in Flights)
            {
                destinations.Add(flight.Destination);                                                                              
            }
            _logger.LogInformation("Retornando resultado del método: GetDestinations");
            return Ok(new List<string>(destinations));

        }

        // <summary>
        ///  Servicio que se encarga de consultar los vuelos cuando se busca solo de ida
        /// </summary>
        [HttpPost("getFlightOneWay")]
        public IActionResult GetFlightsByOrigin([FromBody] FlightFilterDTO filter)
        {
            _logger.LogInformation("Inicio método: GetFlightsByOrigin");
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
            _logger.LogInformation("Retornando resultado del método: GetFlightsByOrigin");
            return Ok(filteredOneWayFlights);
        }

        // <summary>
        ///  Servicio que se encarga de consultar los vuelos cuando se busca de ida y vuelta
        /// </summary>
        [HttpPost("getFlightsRoundTrip")]
        public IActionResult GetFlightsByRoundTrip([FromBody] FlightFilterDTO filter)
        {
            _logger.LogInformation("Inicio método: GetFlightsByRoundTrip");
            if (filter == null || string.IsNullOrEmpty(filter.Origin) || string.IsNullOrEmpty(filter.Destination))
            {
                return BadRequest("Debe proporcionar un origen válido.");
            }

            var filteredOneWayFlights = new List<Flight>();
            var filteredFlightsBack = new List<Flight>();

            filteredOneWayFlights = searchFlights(filter.Origin, filter.Destination);

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
            _logger.LogInformation("Retornando resultado del método: GetFlightsByRoundTrip");
            return Ok(combinedFlights);
        }
    }


} 



