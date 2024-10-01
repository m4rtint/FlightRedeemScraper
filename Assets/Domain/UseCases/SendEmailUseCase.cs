namespace CathayScraperApp.Assets.Domain.UseCases;

public class SendEmailUseCase(MailRepository repository)
{
    private const string TestSubject = "Flight Scraper Test Email";

    public async Task Execute(string email, string subject, string message, string htmlMessage)
    {
        await repository.SendMail(email, subject, message, htmlMessage);
    }

    public async Task ExecuteTest(string email)
    {
        var currentTime = DateTime.Now.ToString("f"); // e.g., "Sunday, September 29, 2024 6:45 PM"
        var message = $"Hello,\n\nThis is a test email sent to {email} on {currentTime}.\n\nBest regards,\nFlight Scraper Team";
        await repository.SendMail(email, TestSubject, message);
    }
}