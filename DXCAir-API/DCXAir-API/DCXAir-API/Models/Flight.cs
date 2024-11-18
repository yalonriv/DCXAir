namespace DCXAir_API.Models
{
    public class Flight
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Price { get; set; }

        public Transport Transport { get; set; }
    }
}
