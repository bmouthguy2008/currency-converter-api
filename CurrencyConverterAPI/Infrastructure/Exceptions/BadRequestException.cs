using CurrencyConverterAPI.Models.Dtos;
using System;
using System.Collections.Generic;

namespace CurrencyConverterAPI.Infrastructure.Exceptions
{
    /// <summary>
    /// Represents a bad request error.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// The list of error messages that fails validation.
        /// </summary>
        public List<CurrencyError> Messages { get; set; }
    }
}
