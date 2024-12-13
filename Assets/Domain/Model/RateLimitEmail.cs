namespace CathayDomain;

public struct RateLimitEmailData
{
    public DateTime FirstSentEmail;
    public DateTime LastSentEmail;
    
    public RateLimitEmailData Copy(DateTime? firstSentEmail = null, DateTime? lastSentEmail = null)
    {
        return new RateLimitEmailData
        {
            FirstSentEmail = firstSentEmail ?? FirstSentEmail,
            LastSentEmail = lastSentEmail ?? LastSentEmail
        };
    }
}