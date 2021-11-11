using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CurrencyConverterAPI.Infrastructure.Serialization;
using CurrencyConverterAPI.Models.Dtos;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using CurrencyConverterAPI.Infrastructure.Exceptions;

namespace CurrencyConverterAPI.Functions
{
    public class ConvertCurrencyFunction
    {
        private readonly ICurrencyConverterService _currencyConverterService;
        private readonly ITextSerializer _textSerializer;
        private readonly IHttpExceptionHandler _exceptionHandler;
        private readonly ILogger<ConvertCurrencyFunction> _log;

        public ConvertCurrencyFunction(ICurrencyConverterService currencyConverterService,
                                       ITextSerializer textSerializer,
                                       IHttpExceptionHandler exceptionHandler,
                                       ILogger<ConvertCurrencyFunction> log)
        {
            _currencyConverterService = currencyConverterService;
            _textSerializer = textSerializer;
            _exceptionHandler = exceptionHandler;
            _log = log;
        }

        [FunctionName("ConvertCurrencyFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("json", typeof(CurrencyRequest),Description = "The request body to convert an amount from one currency to another", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CurrencyResponse), Description = "The exchange rate of the converted currency")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "currencies")] HttpRequest req)
        {
            try
            {
                _log.LogInformation("Started processing a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = _textSerializer.Deserialize<CurrencyRequest>(requestBody);

                ValidateRequest(data);

                var response = await _currencyConverterService.ConvertCurrencyAsync(data);

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex);
            }
            finally
            {
                _log.LogInformation("Finished processing a request.");
            }
        }

        private void ValidateRequest(CurrencyRequest request)
        {
            var messages = new List<CurrencyError>();

            if (string.IsNullOrWhiteSpace(request.To))
            {
                messages.Add(new CurrencyError { Message = "You have entered an invalid 'To' value." });
            }

            if (string.IsNullOrWhiteSpace(request.From))
            {
                messages.Add(new CurrencyError { Message = "You have entered an invalid 'From' value." });
            }

            if(request.Amount <= 0)
            {
                messages.Add(new CurrencyError { Message = "You have entered an invalid amount." });
            }

            if (messages.Any())
            {
                throw new BadRequestException { Messages = messages};
            }
        }
    }
}

