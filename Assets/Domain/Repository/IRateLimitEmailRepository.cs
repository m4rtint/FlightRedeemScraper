using CathayDomain;

namespace CathayScraperApp.Assets.Domain.Repository;

public interface IRateLimitEmailRepository
{
    void UpdateEmailSentTime(string flightId);
    RateLimitEmailData GetEmailSentTimes(string flightId);
}