namespace DCXAir_API.Tests;
using Moq;
using Xunit;
using DCXAir_API.Models;
using DCXAir_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
/// <summary>
///  Clase que determina las pruebas unitarias para el controlador FlightsController
/// </summary>
public class FlightsControllerTest
{
    //Determina el controlador a probar
    private readonly FlightsController _controller;

    /// <summary>
    ///  Constructor de la clase de pruebas
    /// </summary>
    public FlightsControllerTest()
    {
        // Usamos NullLogger en lugar de un logger real
        var logger = NullLogger<FlightsController>.Instance;
        
        _controller = new FlightsController(logger);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetOrigins
    /// </summary>
    [Fact]
    public void GetOrigins_ReturnsOkResult()
    {
        // Ejecutamos el método
        var result = _controller.GetOrigins();

        // Verificamos que el resultado sea un OkObjectResult (200 OK)
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var origins = okResult.Value as List<string>;

        // Verificamos que la lista de orígenes no sea nula
        Assert.NotNull(origins);

        // Comprobamos que al menos haya un origen
        Assert.True(origins.Count > 0);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetDestinations
    /// </summary>
    [Fact]
    public void GetDestinations_ReturnsOkResult()
    {
        // Ejecutamos el método
        var result = _controller.GetDestinations();

        // Verificamos que el resultado sea un OkObjectResult (200 OK)
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var destinations = okResult.Value as List<string>;

        // Verificamos que la lista de destinos no sea nula
        Assert.NotNull(destinations);

        // Comprobamos que al menos haya un destino
        Assert.True(destinations.Count > 0);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetFlightsByOrigin
    /// </summary>
    [Fact]
    public void GetFlightsByOrigin_ReturnsFlights_WhenValidFilter()
    {
        // Crear un filtro de ejemplo
        var filter = new FlightFilterDTO
        {
            Origin = "MZL",
            Destination = "PEI",
            CoinToConvert = "USD"
        };

        // Ejecutamos el método
        var result = _controller.GetFlightsByOrigin(filter);

        // Verificamos que el resultado sea un OkObjectResult (200 OK)
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var flights = okResult.Value as List<Flight>;

        // Verificamos que la lista de vuelos no sea nula y contenga vuelos
        Assert.NotNull(flights);
        Assert.True(flights.Count > 0);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetFlightsByOrigin
    ///  cuando no se encuentran vuelos por un filtro inválido
    /// </summary>
    [Fact]
    public void GetFlightsByOrigin_ReturnsBadRequest_WhenInvalidFilter()
    {
        // Crear un filtro inválido
        var filter = new FlightFilterDTO { Origin = "", Destination = "", CoinToConvert = "USD" };

        // Ejecutamos el método
        var result = _controller.GetFlightsByOrigin(filter);

        // Verificamos que el resultado sea un BadRequestObjectResult (400 Bad Request)
        Assert.IsType<BadRequestObjectResult>(result);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetFlightsByRoundTrip
    /// </summary>
    [Fact]
    public void GetFlightsByRoundTrip_ReturnsFlights_WhenValidFilter()
    {
        // Crear un filtro de ida y vuelta
        var filter = new FlightFilterDTO
        {
            Origin = "MZL",
            Destination = "BOG",
            CoinToConvert = "USD"
        };

        // Ejecutamos el método
        var result = _controller.GetFlightsByRoundTrip(filter);

        // Verificamos que el resultado sea un OkObjectResult (200 OK)
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var flights = okResult.Value as List<Flight>;

        // Verificamos que la lista de vuelos no sea nula y contenga vuelos
        Assert.NotNull(flights);
        Assert.True(flights.Count > 0);
    }

    /// <summary>
    ///  Método que determina las pruebas para el servicio GetFlightsByRoundTrip
    ///  cuando no hay vuelos resultantes por un filtro inválido
    /// </summary>
    [Fact]
    public void GetFlightsByRoundTrip_ReturnsNotFound_WhenNoReturnFlights()
    {
        // Crear un filtro con un destino no válido
        var filter = new FlightFilterDTO
        {
            Origin = "New York",
            Destination = "Nowhere",
            CoinToConvert = "USD"
        };

        // Ejecutamos el método
        var result = _controller.GetFlightsByRoundTrip(filter);

        // Verificamos que el resultado sea un NotFoundObjectResult (404 Not Found)
        Assert.IsType<NotFoundObjectResult>(result);
    }

}
