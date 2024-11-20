namespace DCXAir_API.Utils
{
    public class CoinConversorUtil
    {
        private static decimal tipoCambioUSD = 0.00023m;
        private static decimal tipoCambioEUR = 0.00022m;
        private static decimal tipoCambioMXN = 0.0046m;
        private static decimal tipoCambioGBP = 0.00018m;

        // Método para la conversión de monedas
        public static decimal ConvertCoin(decimal cantidadCOP, string monedaDestino)
        {
            decimal resultado = 0;

            switch (monedaDestino.ToUpper())
            {
                case "USD":
                    resultado = cantidadCOP * tipoCambioUSD;
                    break;

                case "EUR":
                    resultado = cantidadCOP * tipoCambioEUR;
                    break;

                case "GBP":
                    resultado = cantidadCOP * tipoCambioGBP;
                    break;

                case "MXN":
                    resultado = cantidadCOP * tipoCambioMXN;
                    break;


                default:
                    throw new ArgumentException("Moneda no soportada.");
            }

            return resultado;
        }
    }
}
