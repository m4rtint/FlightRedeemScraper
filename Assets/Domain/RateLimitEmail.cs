using CathayDomain;

public class RateLimitEmail
{
    private const string INCREMENT_KEY = "email_increment";
    private const string STORAGE_KEY = "last_email_time";
    private readonly int _allowedEmailsPerMinutes;
    private readonly SimpleStorage _storage;
    private readonly float _timeInMinutesBeforeAllowance;

    public RateLimitEmail(
        int allowedEmailPer,
        float timeBeforeAllowance,
        SimpleStorage storage)
    {
        _timeInMinutesBeforeAllowance = timeBeforeAllowance;
        _allowedEmailsPerMinutes = allowedEmailPer;
        _storage = storage;
    }

    public bool CanEmail()
    {
        var lastSavedTime = _storage.GetFloat(STORAGE_KEY);
        var currentTime = (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        var difference = currentTime - lastSavedTime;
        var isTimePassed = difference > _timeInMinutesBeforeAllowance * 60;
        var isEmailLimitReached = _storage.GetInt(INCREMENT_KEY) >= _allowedEmailsPerMinutes;
        if (isTimePassed) _storage.SetInt(INCREMENT_KEY, 0);
        return isTimePassed && !isEmailLimitReached;
    }

    public void UpdateLastSentEmailTime()
    {
        var currentTime = (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        _storage.SetFloat(STORAGE_KEY, currentTime);
        _storage.SetInt(INCREMENT_KEY, _storage.GetInt(INCREMENT_KEY) + 1);
    }
}