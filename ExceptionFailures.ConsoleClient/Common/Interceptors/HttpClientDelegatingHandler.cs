using ExceptionFailures.ConsoleClient.Common.Exceptions;
using ExceptionFailures.ConsoleClient.Common.Models;
using System.Data;
using System.Net.Http;
using System.Text.Json;

namespace ExceptionFailures.ConsoleClient.Common.Interceptors;

public class HttpClientDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            /*
                * We could process the response here and check for status codes and add failures / flows if not already added by the server. 
            */

            if (!response.IsSuccessStatusCode)
            {
                var contentType = response.Content.Headers.ContentType?.MediaType;
                var content     = await response.Content.ReadAsStringAsync();

                if (contentType == "application/problem+json" && !string.IsNullOrWhiteSpace(content))
                {
                    try
                    { 
                        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                        /*
                            * May be a case statement for different problems for custom dialogs / messages etc. 
                        */ 
                        await Console.Out.WriteLineAsync($"ProblemDetails: {problemDetails}");
                        /*
                            * Now what we are in a typed language without some sort of result type all we can do is throw an exception unless all our methods are bool, true for success false for failure etc. 
                        */
                        throw new ProblemDetailsException(problemDetails!);//Or create and throw yet another custom exception?
                    }
                    catch {throw;}

                }

            }
            return response;
        }
        catch(Exception ex)
        {
            if (ex is not ProblemDetailsException)  await Console.Out.WriteLineAsync("Oops, your guess is as good as mine.");
           
            throw;
        }
    }

    
}