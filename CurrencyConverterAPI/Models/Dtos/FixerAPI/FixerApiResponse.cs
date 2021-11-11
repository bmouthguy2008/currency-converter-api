using System;

namespace CurrencyConverterAPI.Models.Dtos.FixerAPI
{
    /// <summary>
    /// The response details from the Fixer API that contains the currency exchange.
    /// </summary>
    public class FixerApiResponse
    {
        /// <summary>
        /// Returns true or false depending on whether or not your API request has succeeded.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The query details to get the exchange rate.
        /// </summary>
        public FixerAPIQuery Query { get; set; }

        /// <summary>
        /// The info details about the rate.
        /// </summary>
        public FixerAPIInfo Info { get; set; }

        /// <summary>
        /// Returns true if historical rates are used for this conversion.
        /// </summary>
        public string Historical { get; set; }

        /// <summary>
        /// Returns the date (format YYYY-MM-DD) the given exchange rate data was collected.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Returns your conversion result.
        /// </summary>
        public double Result { get; set; }

        /// <summary>
        /// The error details from the Fixer API response.
        /// </summary>
        public FixerAPIError Error { get; set; }
    }
}
