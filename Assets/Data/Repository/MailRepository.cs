public class MailRepository
{
    private readonly MailAPI api;

    public MailRepository(MailAPI api)
    {
        this.api = api;
    }

    public void SendMail(string to, string subject, string message)
    {
        api.SendMail(to, subject, message);
    }
}