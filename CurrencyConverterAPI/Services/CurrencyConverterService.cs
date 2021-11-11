using CurrencyConverterAPI.Infrastructure.Exceptions;
using CurrencyConverterAPI.Infrastructure.Serialization;
using CurrencyConverterAPI.Models.Dtos;
using CurrencyConverterAPI.Models.Dtos.FixerAPI;
using CurrencyConverterAPI.Settings;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Services
{
    /// <summary>
    /// A service to convert a currency amount from one currency to another.
    /// </summary>
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;
        private readonly ITextSerializer _textSerializer;
        private readonly ILogger<CurrencyConverterService> _log;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="textSerializer">A serializer to handle serialization from the external API response.</param>
        /// <param name="log">The logger to log messages.</param>
        public CurrencyConverterService(HttpClient httpClient,
                                        IAppSettings appSettings,
                                        ITextSerializer textSerializer,
                                        ILogger<CurrencyConverterService> log)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
            _textSerializer = textSerializer;
            _log = log;
        }

        /// <summary>
        /// Converts the currency amount from one currency to another and returns.
        /// </summary>
        /// <param name="currencyRequest">The currency details to convert and amount from and to.</param>
        /// <returns>The converted amount in the new currency.</returns>
        public async Task<CurrencyResponse> ConvertCurrencyAsync(CurrencyRequest currencyRequest)
        {
            _log.LogInformation("Starting to call the Fixer API");

            // Get the exchange rate from the API.
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, ConstructFixerApiUrl(currencyRequest));
            var currencyExchangeResponse = await _httpClient.SendAsync(httpRequest);
            var content = await currencyExchangeResponse.Content.ReadAsStringAsync();

            _log.LogInformation("Finished calling the Fixer API");

            var fixerApiResponse = _textSerializer.Deserialize<FixerApiResponse>(content);
            var currencyResponse = new CurrencyResponse();

            if (fixerApiResponse.Success)
            {
                currencyResponse.Rate = fixerApiResponse.Result;
                currencyResponse.Success = true;
            }
            else
            {
                throw new APIApplicationException($"The Fixer API has returned an error, code: {fixerApiResponse.Error.Code}, type: {fixerApiResponse.Error.Type}, info: {fixerApiResponse.Error.Info}");
            }

            return currencyResponse;
        }

        private string ConstructFixerApiUrl(CurrencyRequest currencyRequest)
        {
            return $"{_appSettings.FixerApiBaseUrl}/convert?access_key={_appSettings.FixerApiAccessKey}&from={currencyRequest.From}&to={currencyRequest.To}&amount={currencyRequest.Amount}";
        }
    }
}
