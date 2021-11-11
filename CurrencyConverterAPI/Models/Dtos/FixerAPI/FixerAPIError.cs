namespace CurrencyConverterAPI.Models.Dtos.FixerAPI
{
    /// <summary>
    /// The error details from the Fixer API response.
    /// </summary>
    public class FixerAPIError
    {
        /// <summary>
        /// The error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The type of the error.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The description of the error. 
        /// </summary>
        public string Info { get; set; }
    }
}
