public class SendEmailUseCase
{
    private const string EMAIL = "m4rtin.t@gmail.com";
    private const string SUBJECT = "FOUND SEATS ON DAY";
    private readonly RateLimitEmail _rateLimitEmail;
    private readonly MailRepository _repository;

    public SendEmailUseCase(MailRepository repository, RateLimitEmail rateLimitEmail)
    {
        _repository = repository;
        _rateLimitEmail = rateLimitEmail;
    }

    public void Execute(string message)
    {
        if (_rateLimitEmail.CanEmail())
        {
            _repository.SendMail(EMAIL, SUBJECT, message);
            _rateLimitEmail.UpdateLastSentEmailTime();
        }
        else
        {
            DebugLogger.Log("Error: Rate limit exceeded.");
        }
    }
}