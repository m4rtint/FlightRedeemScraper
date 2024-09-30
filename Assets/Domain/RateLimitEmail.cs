using CathayDomain;

namespace CathayScraperApp.Assets.Domain;

public class RateLimitEmail(
    int allowedEmailPer,
    float timeBeforeAllowance,
    SimpleStorage storage)
{
    private const string INCREMENT_KEY = "email_increment";
    private const string STORAGE_KEY = "last_email_time";

    public bool CanEmail()
    {
        var lastSavedTime = storage.GetFloat(STORAGE_KEY);
        var currentTime = (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        var difference = currentTime - lastSavedTime;
        var isTimePassed = difference > timeBeforeAllowance * 60;
        var isEmailLimitReached = storage.GetInt(INCREMENT_KEY) >= allowedEmailPer;
        if (isTimePassed) storage.SetInt(INCREMENT_KEY, 0);
        return isTimePassed && !isEmailLimitReached;
    }

    public void UpdateLastSentEmailTime()
    {
        var currentTime = (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        storage.SetFloat(STORAGE_KEY, currentTime);
        storage.SetInt(INCREMENT_KEY, storage.GetInt(INCREMENT_KEY) + 1);
    }
}