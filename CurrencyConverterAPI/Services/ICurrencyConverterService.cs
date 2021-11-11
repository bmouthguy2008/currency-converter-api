using CurrencyConverterAPI.Models.Dtos;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Services
{
    /// <summary>
    /// An interface to connect an external currency converter API.
    /// </summary>
    public interface ICurrencyConverterService
    {
        /// <summary>
        /// Converts the currency amount from one currency to another and returns.
        /// </summary>
        /// <param name="currencyRequest">The currency details to convert and amount from and to.</param>
        /// <returns>The converted amount in the new currency.</returns>
        Task<CurrencyResponse> ConvertCurrencyAsync(CurrencyRequest currencyRequest);
    }
}
