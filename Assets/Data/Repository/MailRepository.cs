public class MailRepository(MailAPI api)
{
    public async Task SendMail(string to, string subject, string message)
    {
        await api.SendMail(to, subject, message);
    }
}