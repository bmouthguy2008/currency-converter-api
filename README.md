# currency-converter-api
An API to convert a currency amount from one currency to another currency.


Enhancements to be made to make it a professional service:

Add more caught exceptions in the HttpExceptionHandler to catch and handle other types of exceptions, e.g. Forbidden, Unauthorized. I have just added the BadRequestException to show the structure.

Add API Management in front of the function app that includes rate limits and lock down the function app so only APIM will be able to call it.

Add the azure application configuration service to handle the app settings and possible feature toggles.

Add ARM templates to build the CI/CD pipeline and auto deploy the resources to azure.

Add retry logic when calling the Fixer API to handle transient errors.

Add some more additional logging to log the request time for the Fixer API and also the whole request for the API itself.

Add more validation checks in the function to validate the request to make sure the currency abbreviations are correct before sending off to the Fixer API. This will reduce calling the Fixer API.

Add healthcheck endpoint. This could also ping/call the Fixer API to verify the downstream API is up and running.
