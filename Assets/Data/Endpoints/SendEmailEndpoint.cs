using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

public class SendEmailEndpont
{
    private const string MAILGUN_DOMAIN = "martinproto.com";
    private const string MAILGUN_API_URL = "https://api.mailgun.net/v3/";

    public async Task SendEmail(string to, string subject, string textBody, string? htmlBody = null) // htmlBody is optional
    {
        using var client = new HttpClient();
        
        // Get API key from environment variable
        string apiKey = Environment.GetEnvironmentVariable("MailGun-ApiKey", EnvironmentVariableTarget.User);
        if (string.IsNullOrEmpty(apiKey))
        {
            DebugLogger.Log("MailGun API key is not set.");
            return;
        }
        
        var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

        // Construct the form data
        var formData = new List<KeyValuePair<string, string>>
        {
            new("from", "postmaster@" + MAILGUN_DOMAIN),
            new("to", to),
            new("subject", subject),
            new("text", textBody)  // Plain text version
        };

        // Conditionally add the HTML body if provided
        if (!string.IsNullOrEmpty(htmlBody))
        {
            formData.Add(new KeyValuePair<string, string>("html", htmlBody));
        }

        // Encode the form data
        var encodedFormData = new FormUrlEncodedContent(formData);

        try
        {
            // Send the POST request to Mailgun's API
            var response = await client.PostAsync($"{MAILGUN_API_URL}{MAILGUN_DOMAIN}/messages", encodedFormData);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                DebugLogger.Log($"Email sent successfully to {to}!");
            }
            else
            {
                DebugLogger.Log($"Failed to send email. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            DebugLogger.Log($"Error sending email: {ex.Message}");
        }
    }
}
