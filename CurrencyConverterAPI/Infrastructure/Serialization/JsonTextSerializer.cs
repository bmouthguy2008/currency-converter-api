using Newtonsoft.Json;

namespace CurrencyConverterAPI.Infrastructure.Serialization
{
    /// <summary>
    /// Serialization for json text data.
    /// </summary>
    public class JsonTextSerializer : ITextSerializer
    {
        /// <summary>
        /// Deserializes data to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <returns>The type of object deserialized to.</returns>
        public T Deserialize<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
