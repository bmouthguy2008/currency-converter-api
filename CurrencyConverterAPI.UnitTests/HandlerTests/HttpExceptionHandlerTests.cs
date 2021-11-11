using CurrencyConverterAPI.Infrastructure.Exceptions;
using CurrencyConverterAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace CurrencyConverterAPI.UnitTests.HandlerTests
{
    [TestClass]
    public class HttpExceptionHandlerTests
    {
        [TestMethod]
        public void GivenIHaveAnAPIApplicationException_WhenIHandleTheException_ThenTheStatusCodeIsInternalServerError()
        {
            var mockLogger = new Mock<ILogger<HttpExceptionHandler>>();
            var exception = new APIApplicationException("An internal server error has happened.");

            var handler = new HttpExceptionHandler(mockLogger.Object);
            var response = handler.HandleException(exception) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void GivenIHaveAnAPIApplicationException_WhenIHandleTheException_ThenReturnsAnErrorMessage()
        {
            var mockLogger = new Mock<ILogger<HttpExceptionHandler>>();
            var exception = new APIApplicationException("An internal server error has happened.");

            var handler = new HttpExceptionHandler(mockLogger.Object);
            var response = handler.HandleException(exception) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Value, typeof(CurrencyResponse));

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("Something has gone wrong, please contact your customer administrator.", currencyResponse.Errors[0].Message);
        }

        [TestMethod]
        public void GivenIHaveABadRequestException_WhenIHandleTheException_ThenTheStatusCodeIsBadRequest()
        {
            var mockLogger = new Mock<ILogger<HttpExceptionHandler>>();
            var errorMessages = new List<CurrencyError>()
            {
                new CurrencyError {Message = "You have entered an invalid 'To' value." }
            };

            var exception = new BadRequestException()
            {
                Messages = errorMessages
            };

            var handler = new HttpExceptionHandler(mockLogger.Object);
            var response = handler.HandleException(exception) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GivenIHaveABadRequestException_WhenIHandleTheException_ThenReturnsAnErrorMessage()
        {
            var mockLogger = new Mock<ILogger<HttpExceptionHandler>>();
            var errorMessages = new List<CurrencyError>()
            {
                new CurrencyError { Message = "You have entered an invalid 'To' value." }
            };

            var exception = new BadRequestException()
            {
                Messages = errorMessages
            };

            var handler = new HttpExceptionHandler(mockLogger.Object);
            var response = handler.HandleException(exception) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("You have entered an invalid 'To' value.", currencyResponse.Errors[0].Message);            
        }
    }
}
