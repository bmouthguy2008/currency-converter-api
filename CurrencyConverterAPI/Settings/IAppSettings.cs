namespace CurrencyConverterAPI.Settings
{
    /// <summary>
    /// The settings for the application.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// The base Fixer API to retrieve the currency exchange rate.
        /// </summary>
        string FixerApiBaseUrl { get; }

        /// <summary>
        /// The access key for the Fixer API.
        /// </summary>
        string FixerApiAccessKey { get; }
    }
}
