using System.Collections.Generic;

namespace CurrencyConverterAPI.Models.Dtos
{
    /// <summary>
    /// The response containing the converted currency rate.
    /// </summary>
    public class CurrencyResponse
    {
        /// <summary>
        /// If the request submitted was successful or not.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The currency rate.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// A list of errors that have been returned.
        /// </summary>
        public List<CurrencyError> Errors { get; set; }
    }
}
