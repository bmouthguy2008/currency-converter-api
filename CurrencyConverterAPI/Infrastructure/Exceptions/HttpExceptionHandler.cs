using CurrencyConverterAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CurrencyConverterAPI.Infrastructure.Exceptions
{
    /// <summary>
    /// A handler for handling exceptions.
    /// </summary>
    public class HttpExceptionHandler : IHttpExceptionHandler
    {
        private readonly ILogger<HttpExceptionHandler> _log;

        /// <summary>
        /// Default constructor for the handler.
        /// </summary>
        /// <param name="log">The logger to log messages.</param>
        public HttpExceptionHandler(ILogger<HttpExceptionHandler> log)
        {
            _log = log;
        }

        /// <summary>
        /// Handles the thrown exception.
        /// </summary>
        /// <param name="exception">The exception that has been thrown.</param>
        /// <returns>The http action result to send to the client.</returns>
        public IActionResult HandleException(Exception exception)
        {
            _log.LogError(exception, exception.Message);

            CurrencyResponse response;
            int statusCode;

            switch (exception)
            {
                // 400 Bad Request.
                case BadRequestException ex:

                    response = new CurrencyResponse
                    {
                        Errors = ex.Messages
                    };

                    statusCode = StatusCodes.Status400BadRequest;

                    break;
                default:
                    // Handle any unhandled exceptions.
                    response = new CurrencyResponse
                    {
                        Errors = new List<CurrencyError>
                        {
                            new CurrencyError { Message = "Something has gone wrong, please contact your customer administrator." }
                        }
                    };

                    statusCode = StatusCodes.Status500InternalServerError;

                    break;
            }

            return new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }
    }
}
