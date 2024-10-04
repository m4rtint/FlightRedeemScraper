using System.Net.Http;
using System.Net.Http.Headers;

public class VerifyMailGunApiEndpoint
{
    private static readonly HttpClient client = new HttpClient();

    // The Mailgun API endpoint you want to use (this example uses a simple domain info endpoint)
    private const string MailgunApiBaseUri = "https://api.mailgun.net/v3/domains";

    // This method verifies the provided Mailgun API key by making a request to Mailgun
    public async Task<bool> VerifyApi(string apiKey)
    {
        try
        {
            // Prepare the API request
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($"api:{apiKey}")));

            // Make a request to the Mailgun API
            HttpResponseMessage response = await client.GetAsync(MailgunApiBaseUri);

            // Check the status code
            if (response.IsSuccessStatusCode)
            {
                // If we get a success status code (2xx), return true
                return true;
            }
            else
            {
                // Log the failure reason using DebugLogger
                DebugLogger.Log($"Mailgun API verification failed. Status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            // Log network errors or other request exceptions using DebugLogger
            DebugLogger.Log($"Request exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Log general exceptions using DebugLogger
            DebugLogger.Log($"General exception: {ex.Message}");
        }

        // If we reach this point, the verification failed
        return false;
    }
}

