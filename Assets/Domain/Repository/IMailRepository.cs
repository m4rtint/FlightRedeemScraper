namespace CathayScraperApp.Assets.Domain.Repository;

public interface IMailRepository
{
    public Task SendMail(string to, string subject, string message, string? htmlMessage = null);
    public Task<bool> VerifyAPI(string apiKey);
}