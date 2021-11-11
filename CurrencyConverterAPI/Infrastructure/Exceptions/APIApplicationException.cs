using System;

namespace CurrencyConverterAPI.Infrastructure.Exceptions
{
    /// <summary>
    /// Represents an API error.
    /// </summary>
    public class APIApplicationException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="message">The error message.</param>
        public APIApplicationException(string message) : base(message)
        {
        }
    }
}
