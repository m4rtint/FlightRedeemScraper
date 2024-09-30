namespace CathayScraperApp.Assets.Domain.UseCases;

public class SendEmailUseCase(MailRepository repository)
{
    private const string TestSubject = "Flight Scraper Test Email";

    public void Execute(string email, string subject, string message)
    {
        repository.SendMail(email, subject, message);
    }

    public void ExecuteTest(string email)
    {
        string currentTime = DateTime.Now.ToString("f"); // e.g., "Sunday, September 29, 2024 6:45 PM"
        string message = $"Hello,\n\nThis is a test email sent to {email} on {currentTime}.\n\nBest regards,\nFlight Scraper Team";
        repository.SendMail(email, TestSubject, message);
    }
}