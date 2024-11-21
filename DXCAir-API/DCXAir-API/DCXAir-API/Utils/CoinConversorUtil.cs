namespace DCXAir_API.Utils
{
    /// <summary>
    ///  Clase encargada de realizar la administración de conversión de monedas.
    /// </summary>
    public class CoinConversorUtil
    {
        //Determina el tipo de cambio cuando se va a convertir de pesos a dólares USD
        private static decimal tipoCambioUSD = 0.00023m;
        //Determina el tipo de cambio cuando se va a convertir de pesos a euros EUR
        private static decimal tipoCambioEUR = 0.00022m;
        //Determina el tipo de cambio cuando se va a convertir de pesos colombianos a pesos mexicanos MZN
        private static decimal tipoCambioMXN = 0.0046m;
        //Determina el tipo de cambio cuando se va a convertir de pesos a libra GBP
        private static decimal tipoCambioGBP = 0.00018m;

        /// <summary>
        ///  Método encargado de convertir de pesos colombianos  a la moneda indicada
        ///  según los parámetros
        /// </summary>
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
