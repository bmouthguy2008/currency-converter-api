namespace CurrencyConverterAPI.Infrastructure.Serialization
{
    /// <summary>
    /// Serialization for text data.
    /// </summary>
    public interface ITextSerializer
    {
        /// <summary>
        /// Deserializes data to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <returns>The type of object deserialized to.</returns>
        T Deserialize<T>(string data) where T : class;
    }
}
