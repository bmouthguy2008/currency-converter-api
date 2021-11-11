namespace CurrencyConverterAPI.Models.Dtos.FixerAPI
{
    /// <summary>
    /// The Fixer API rates for each currency.
    /// </summary>
    public class FixerAPIRates
    {
        /// <summary>
        /// The currency type.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The rate of the currency.
        /// </summary>
        public double Rate { get; set; }
    }
}
