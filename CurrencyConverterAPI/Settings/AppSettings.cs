using System;

namespace CurrencyConverterAPI.Settings
{
    /// <summary>
    /// The settings for the application.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        /// <summary>
        /// The base Fixer API to retrieve the currency exchange rate.
        /// </summary>
        public string FixerApiBaseUrl => GetSetting("FixerApiBaseUrl");

        /// <summary>
        /// The access key for the Fixer API.
        /// </summary>
        public string FixerApiAccessKey => GetSetting("FixerApiAccessKey");

        private string GetSetting(string key)
        {
            var applicationSetting = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);

            if (!string.IsNullOrWhiteSpace(applicationSetting))
            {
                return applicationSetting;
            }

            throw new Exception($"App setting '{key}' is not set");
        }
    }
}
