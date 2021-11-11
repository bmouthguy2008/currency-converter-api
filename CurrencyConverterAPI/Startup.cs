using CurrencyConverterAPI;
using CurrencyConverterAPI.Infrastructure.Exceptions;
using CurrencyConverterAPI.Infrastructure.Serialization;
using CurrencyConverterAPI.Services;
using CurrencyConverterAPI.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace CurrencyConverterAPI
{
    public class Startup : FunctionsStartup
    {
        private AppSettings _appSettings;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _appSettings = new AppSettings();

            builder.Services.AddHttpClient<ICurrencyConverterService, CurrencyConverterService>(client =>
            {
                client.BaseAddress = new Uri(_appSettings.FixerApiBaseUrl);
            });

            builder.Services.AddScoped<IHttpExceptionHandler, HttpExceptionHandler>();
            builder.Services.AddSingleton<IAppSettings, AppSettings>();
            builder.Services.AddSingleton<ITextSerializer, JsonTextSerializer>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters().AddJsonOptions(options => {
                // Adding json option to ignore null values.
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }
    }
}
