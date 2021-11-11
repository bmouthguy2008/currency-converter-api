using CurrencyConverterAPI.Infrastructure.Exceptions;
using CurrencyConverterAPI.Infrastructure.Serialization;
using CurrencyConverterAPI.Models.Dtos;
using CurrencyConverterAPI.Services;
using CurrencyConverterAPI.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.UnitTests.ServiceTests
{
    [TestClass]
    public class CurrencyConverterServiceTests
    {
        private CurrencyConverterService _currencyConverterService;

        public void Initialize(StringContent content)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            };

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync",
                                                         ItExpr.IsAny<HttpRequestMessage>(),
                                                         ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(response);

            var mockHttpClient = new Mock<HttpClient>(mockHandler.Object);

            var mockAppSettings = new Mock<IAppSettings>();            
            mockAppSettings.Setup(a => a.FixerApiBaseUrl).Returns("http://my--fake-test-uri/api");
            mockAppSettings.Setup(a => a.FixerApiAccessKey).Returns("Test_Key");

            var serializer = new JsonTextSerializer();
            var mockLogger = new Mock<ILogger<CurrencyConverterService>>();

            _currencyConverterService = new CurrencyConverterService(mockHttpClient.Object, 
                                                                     mockAppSettings.Object,
                                                                     serializer, 
                                                                     mockLogger.Object);
        }

        [TestMethod]
        public async Task GivenIHaveACurrencyAmount_WhenIConvertAmountToAnotherCurrency_ThenTheAmountIsConverted()
        {
            var Content = new StringContent(@"{ ""success"": true, ""query"": { ""from"": ""GBP"", ""to"": ""USD"", ""amount"": 10}, ""info"": { ""timestamp"": 1636628777, ""rate"": 1.339414}, ""date"": ""2021-11-11"", ""result"": 13.39414}");

            Initialize(Content);

            var currencyRequest = new CurrencyRequest
            {
                To = "USD",
                From = "GBP",
                Amount = 100
            };

            var response = await _currencyConverterService.ConvertCurrencyAsync(currencyRequest);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(13.39414, response.Rate);
        }

        [ExpectedException(typeof(APIApplicationException))]
        [TestMethod]
        public async Task GivenIHaveAnInvalidToCurrency_WhenIConvertAmountToAnotherCurrency_ThenAnExceptionIsThrown()
        {
            var Content = new StringContent(@"{ ""success"": false, ""error"": { ""code"": 402, ""type"": ""invalid_to_currency"", ""info"": ""You have entered an invalid \""to\"" property. [Example: to=GBP]""}}");
                        
            Initialize(Content);

            var currencyRequest = new CurrencyRequest
            {
                To = "USD1",
                From = "GBP",
                Amount = 100
            };

            try
            {
                var response = await _currencyConverterService.ConvertCurrencyAsync(currencyRequest);
            }
            catch (APIApplicationException ex)
            {
                Assert.AreEqual("The Fixer API has returned an error, code: 402, type: invalid_to_currency, info: You have entered an invalid \"to\" property. [Example: to=GBP]", ex.Message);
                throw;
            }            
        }

        [ExpectedException(typeof(APIApplicationException))]
        [TestMethod]
        public async Task GivenIHaveAnInvalidFromCurrency_WhenIConvertAmountToAnotherCurrency_ThenAnExceptionIsThrown()
        {
            var Content = new StringContent(@"{ ""success"": false, ""error"": { ""code"": 402, ""type"": ""invalid_from_currency"", ""info"": ""You have entered an invalid \""from\"" property. [Example: to=GBP]""}}");

            Initialize(Content);

            var currencyRequest = new CurrencyRequest
            {
                To = "USD",
                From = "GBP1",
                Amount = 100
            };

            try
            {
                var response = await _currencyConverterService.ConvertCurrencyAsync(currencyRequest);
            }
            catch (APIApplicationException ex)
            {
                Assert.AreEqual("The Fixer API has returned an error, code: 402, type: invalid_from_currency, info: You have entered an invalid \"from\" property. [Example: to=GBP]", ex.Message);
                throw;
            }
        }

        [ExpectedException(typeof(APIApplicationException))]
        [TestMethod]
        public async Task GivenIHaveAnInvalidZeroAmount_WhenIConvertAmountToAnotherCurrency_ThenAnExceptionIsThrown()
        {
            var Content = new StringContent(@"{ ""success"": false, ""error"": { ""code"": 403, ""type"": ""invalid_conversion_amount"", ""info"": ""You have not specified an amount to be converted. [Example: amount=5]""}}");

            Initialize(Content);

            var currencyRequest = new CurrencyRequest
            {
                To = "USD",
                From = "GBP",
                Amount = 0
            };

            try
            {
                var response = await _currencyConverterService.ConvertCurrencyAsync(currencyRequest);
            }
            catch (APIApplicationException ex)
            {
                Assert.AreEqual("The Fixer API has returned an error, code: 403, type: invalid_conversion_amount, info: You have not specified an amount to be converted. [Example: amount=5]", ex.Message);
                throw;
            }
        }

        [ExpectedException(typeof(APIApplicationException))]
        [TestMethod]
        public async Task GivenIHaveAnInvalidNegativeAmount_WhenIConvertAmountToAnotherCurrency_ThenAnExceptionIsThrown()
        {
            var Content = new StringContent(@"{ ""success"": false, ""error"": { ""code"": 403, ""type"": ""invalid_conversion_amount"", ""info"": ""You have not specified an amount to be converted. [Example: amount=5]""}}");

            Initialize(Content);

            var currencyRequest = new CurrencyRequest
            {
                To = "USD",
                From = "GBP",
                Amount = -10
            };

            try
            {
                var response = await _currencyConverterService.ConvertCurrencyAsync(currencyRequest);
            }
            catch (APIApplicationException ex)
            {
                Assert.AreEqual("The Fixer API has returned an error, code: 403, type: invalid_conversion_amount, info: You have not specified an amount to be converted. [Example: amount=5]", ex.Message);
                throw;
            }
        }
    }
}
