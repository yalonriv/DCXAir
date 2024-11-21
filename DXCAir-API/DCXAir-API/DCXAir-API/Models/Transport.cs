namespace DCXAir_API.Models
{
    /// <summary>
    ///  Clase que determina el transporte en el que se realizará el vuelo
    /// </summary>
    public class Transport
    {
        // Identificador de aerolínea
        public string FlightCarrier { get; set; }
        //Determina el número de vuelo
        public string FlightNumber { get; set; }
    }
}
