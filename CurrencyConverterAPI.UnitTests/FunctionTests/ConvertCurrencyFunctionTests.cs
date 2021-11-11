using CurrencyConverterAPI.Functions;
using CurrencyConverterAPI.Infrastructure.Exceptions;
using CurrencyConverterAPI.Infrastructure.Serialization;
using CurrencyConverterAPI.Models.Dtos;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.UnitTests.FunctionTests
{
    [TestClass]
    public class ConvertCurrencyFunctionTests
    {
        private ConvertCurrencyFunction _function;        

        [TestInitialize]
        public void Initialize()
        {
            var validCurrencyResponse = new CurrencyResponse()
            {
                Success = true,
                Rate = 1.31
            };

            var errorCurrencyResponse = new CurrencyResponse()
            {
                Errors = new System.Collections.Generic.List<CurrencyError>
                {
                    new CurrencyError { Message = "You have entered an invalid 'To' value." }
                }
            };

            var mockFunctionLog = new Mock<ILogger<ConvertCurrencyFunction>>();
            var mockExceptionHandlerLog = new Mock<ILogger<HttpExceptionHandler>>();
            var mockCurrencyConverterService = new Mock<ICurrencyConverterService>();
            mockCurrencyConverterService.Setup(cs => cs.ConvertCurrencyAsync(It.IsAny<CurrencyRequest>())).Returns(Task.FromResult(validCurrencyResponse));

            var textSerializer = new JsonTextSerializer();
            var exceptionHandler = new HttpExceptionHandler(mockExceptionHandlerLog.Object);

            _function = new ConvertCurrencyFunction(mockCurrencyConverterService.Object, 
                                                    textSerializer,
                                                    exceptionHandler,
                                                    mockFunctionLog.Object);
        }

        [TestMethod]
        public async Task GivenIHaveACurrencyAmount_WhenIConvertAmountToAnotherCurrency_ThenTheAmountIsConverted()
        {
            var currencyRequest = new CurrencyRequest()
            {
                From = "GBP",
                To = "USD",
                Amount = 10
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(currencyRequest);

            var response = await _function.Run(mockRequest.Object) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Value, typeof(CurrencyResponse));

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsTrue(currencyResponse.Success);
            Assert.AreEqual(1.31, currencyResponse.Rate);
        }

        [TestMethod]
        public async Task GivenIHaveAnEmptyToCurrency_WhenIConvertAmountToAnotherCurrency_ThenTheStatusCodeIsBadRequest()
        {
            var currencyRequest = new CurrencyRequest()
            {
                From = "GBP",
                To = "",
                Amount = 10
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(currencyRequest);

            var response = await _function.Run(mockRequest.Object) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("You have entered an invalid 'To' value.", currencyResponse.Errors[0].Message);
        }

        [TestMethod]
        public async Task GivenIHaveAnEmptyFromCurrency_WhenIConvertAmountToAnotherCurrency_ThenTheStatusCodeIsBadRequest()
        {
            var currencyRequest = new CurrencyRequest()
            {
                From = "",
                To = "USD",
                Amount = 10
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(currencyRequest);

            var response = await _function.Run(mockRequest.Object) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("You have entered an invalid 'From' value.", currencyResponse.Errors[0].Message);
        }

        [TestMethod]
        public async Task GivenIHaveAnZeroAmountCurrency_WhenIConvertAmountToAnotherCurrency_ThenTheStatusCodeIsBadRequest()
        {
            var currencyRequest = new CurrencyRequest()
            {
                From = "GBP",
                To = "USD",
                Amount = 0
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(currencyRequest);

            var response = await _function.Run(mockRequest.Object) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("You have entered an invalid amount.", currencyResponse.Errors[0].Message);
        }

        [TestMethod]
        public async Task GivenIHaveANegativeAmountCurrency_WhenIConvertAmountToAnotherCurrency_ThenTheStatusCodeIsBadRequest()
        {
            var currencyRequest = new CurrencyRequest()
            {
                From = "GBP",
                To = "USD",
                Amount = -10
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(currencyRequest);

            var response = await _function.Run(mockRequest.Object) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var currencyResponse = response.Value as CurrencyResponse;

            Assert.IsFalse(currencyResponse.Success);
            Assert.AreEqual("You have entered an invalid amount.", currencyResponse.Errors[0].Message);
        }

        private static Mock<HttpRequest> CreateMockRequest(object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}
