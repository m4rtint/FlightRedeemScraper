using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

public class SendEmailEndpont
{
    private const string MAILGUN_DOMAIN = "martinproto.com";
    private const string MAILGUN_API_URL = "https://api.mailgun.net/v3/";

    public async void SendEmail(string to, string subject, string body)
    {
        using (var client = new HttpClient())
        {
            // Set up basic authentication
            string apiKey = Environment.GetEnvironmentVariable("MailGun-ApiKey", EnvironmentVariableTarget.User);
            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            // Construct the form data
            var formData = new List<KeyValuePair<string, string>>
            {
                new("from", "postmaster@" + MAILGUN_DOMAIN),
                new("to", to),
                new("subject", subject),
                new("text", body)
            };

            // Encode the form data
            var encodedFormData = new FormUrlEncodedContent(formData);

            try
            {
                // Send the POST request to Mailgun's API
                var response = await client.PostAsync($"{MAILGUN_API_URL}{MAILGUN_DOMAIN}/messages", encodedFormData);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                    DebugLogger.Log("Email sent successfully!");
                else
                    DebugLogger.Log(
                        $"Failed to send email. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                DebugLogger.Log($"Error sending email: {ex.Message}");
            }
        }
    }
}