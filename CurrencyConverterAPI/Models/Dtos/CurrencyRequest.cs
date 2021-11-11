namespace CurrencyConverterAPI.Models.Dtos
{
    /// <summary>
    /// The request containing the currency details to convert.
    /// </summary>
    public class CurrencyRequest
    {
        /// <summary>
        /// The currency to convert the amount to.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The currency to convert the amount from.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The amount to convert.
        /// </summary>
        public double Amount { get; set; }
    }
}
