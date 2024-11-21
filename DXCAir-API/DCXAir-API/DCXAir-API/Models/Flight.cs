namespace DCXAir_API.Models
{
    /// <summary>
    ///  Clase que almacena la información de un vuelo
    /// </summary>
    public class Flight
    {
        //Determina el origen del vuelo
        public string Origin { get; set; }

        //Determina el destino del vuelo
        public string Destination { get; set; }

        //Determina el precio del vuelo
        public string Price { get; set; }

        //Determina el transporte en el que se realizará el vuelo
        public Transport Transport { get; set; }
    }
}
