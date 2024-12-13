using CathayScraperApp.Assets.Domain.Repository;

namespace CathayScraperApp.Assets.Domain.UseCases;

public class UpdateRateLimitEmailUseCase(IRateLimitEmailRepository rateLimitEmailRepository)
{
    public void Execute(string flightId)
    {
        rateLimitEmailRepository.UpdateEmailSentTime(flightId);
    }
}