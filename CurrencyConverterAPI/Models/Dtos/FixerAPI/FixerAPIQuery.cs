namespace CurrencyConverterAPI.Models.Dtos.FixerAPI
{
    /// <summary>
    /// The Fixer API query details.
    /// </summary>
    public class FixerAPIQuery
    {
        /// <summary>
        /// The currency to convert from.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The currency to convert to.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The amount to convert.
        /// </summary>
        public double Amount { get; set; }
    }
}
