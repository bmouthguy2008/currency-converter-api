namespace CurrencyConverterAPI.Models.Dtos.FixerAPI
{
    /// <summary>
    /// The Fixer API info details.
    /// </summary>
    public class FixerAPIInfo
    {
        /// <summary>
        /// Returns the exact date and time (UNIX time stamp) the given exchange rare was collected.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Returns the exchange rate used for the conversion.
        /// </summary>
        public double Rate { get; set; }
    }
}
