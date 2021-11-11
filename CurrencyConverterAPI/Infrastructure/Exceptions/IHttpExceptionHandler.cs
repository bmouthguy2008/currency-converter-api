using Microsoft.AspNetCore.Mvc;
using System;

namespace CurrencyConverterAPI.Infrastructure.Exceptions
{
    /// <summary>
    /// A handler for handling exceptions.
    /// </summary>
    public interface IHttpExceptionHandler
    {
        /// <summary>
        /// Handles the thrown exception.
        /// </summary>
        /// <param name="exception">The exception that has been thrown.</param>
        /// <returns>The http action result to send to the client.</returns>
        IActionResult HandleException(Exception exception);
    }
}
