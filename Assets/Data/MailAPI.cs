public class MailAPI
{
    public Task SendMail(string to, string subject, string message)
    {
        var endpoint = new SendEmailEndpont();
        endpoint.SendEmail(to, subject, message);
        return Task.CompletedTask;
    }
}