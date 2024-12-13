using CathayDomain;
using CathayScraperApp.Assets.Domain.Repository;

namespace CathayScraperApp.Assets.Data.Repository;

public class DefaultRateLimitEmailRepository: IRateLimitEmailRepository
{
    private readonly Dictionary<string, RateLimitEmailData> _emailSendCount = new Dictionary<string, RateLimitEmailData>();
    
    public void UpdateEmailSentTime(string flightId)
    {
        if (_emailSendCount.TryGetValue(flightId, out var emailSentTimes))
        {
            _emailSendCount[flightId] = emailSentTimes.Copy(lastSentEmail: DateTime.Now);
        }
        else
        {
            _emailSendCount.Add(flightId, new RateLimitEmailData
            {
                FirstSentEmail = DateTime.Now,
                LastSentEmail = DateTime.Now
            });
        }
    }
    
    public RateLimitEmailData GetEmailSentTimes(string flightId)
    {
        if (_emailSendCount.TryGetValue(flightId, out var emailSentTimes))
        {
            return emailSentTimes;
        }

        return new RateLimitEmailData
        {
            FirstSentEmail = DateTime.Now,
            LastSentEmail = DateTime.Now
        };
    }
}