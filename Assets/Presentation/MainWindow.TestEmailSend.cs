using System.Windows;
using CathayDomain;

namespace CathayScraperApp;

public partial class MainWindow
{
    private readonly SimpleStorage _mockStorage = new("mock");

    private void TestSendEmailButtonClick(object sender, RoutedEventArgs e)
    {
        var api = new MailAPI();
        var mailRepository = new MailRepository(api);
        var rateLimitEmail = new RateLimitEmail(1, 1,
            _mockStorage);
        var sendEmailUseCase = new SendEmailUseCase(mailRepository, rateLimitEmail);
        sendEmailUseCase.Execute("Test Email From Cathay Scraper App.");
    }
}