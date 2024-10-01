public class MailAPI
{
    public async Task SendMail(string to, string subject, string message, string? htmlMessage = null)
    {
        var endpoint = new SendEmailEndpont();
        await endpoint.SendEmail(to, subject, message, htmlMessage);
    }
}