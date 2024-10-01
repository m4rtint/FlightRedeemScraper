public class MailRepository(MailAPI api)
{
    public async Task SendMail(string to, string subject, string message, string? htmlMessage = null)
    {
        await api.SendMail(to, subject, message, htmlMessage);
    }
}