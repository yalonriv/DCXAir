namespace DCXAir_API.Models
{
    /// <summary>
    ///  Clase que determina los filtros para buscar los vuelos
    /// </summary>
    public class FlightFilterDTO
    {
        //Determina el origen requerido para el vuelo
        public string Origin { get; set; }

        //Determina el destino requerido para el vuelo
        public string Destination { get; set; }

        //Determina el tipo de moneda en el que se requiere conocer el precio del vuelo
        public string CoinToConvert { get; set; }
    }
}
