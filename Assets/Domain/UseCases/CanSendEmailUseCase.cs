using CathayDomain;
using CathayScraperApp.Assets.Domain.Repository;

namespace CathayScraperApp.Assets.Domain.UseCases;

public class CanSendEmailUseCase(IRateLimitEmailRepository rateLimitEmailRepository)
{
    private const int HOURS_BEFORE_SWITCHING_TO_SLOW_RATE = 2;

    /*
     * Send an email at normal rate for 2 hours, before switching to every 3 hours
     */
    public bool Execute(FlightEntryToScanRequest request)
    {
        // Get the count of emails sent for this flight
        var data = rateLimitEmailRepository.GetEmailSentTimes(request.Id);
        // If last email sent was within 2 hour of first send, send the email.
        if (DateTime.Now - data.FirstSentEmail < TimeSpan.FromHours(HOURS_BEFORE_SWITCHING_TO_SLOW_RATE))
        {
            return true;
        }
        // But if it's more than 2 hours from first send, send the email only after 3 hours from last sent email
        return DateTime.Now - data.LastSentEmail > TimeSpan.FromHours(3);
    }
}