using CathayScraperApp.Assets.Domain.Repository;

public class MailRepository(MailAPI api): IMailRepository
{
    public async Task SendMail(string to, string subject, string message, string? htmlMessage = null)
    {
        await api.SendMail(to, subject, message, htmlMessage);
    }

    public async Task<bool> VerifyAPI(string apiKey)
    {
        return await api.VerifyAPI(apiKey);
    }
}