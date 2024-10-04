using CathayScraperApp.Assets.Domain.Repository;

namespace CathayScraperApp.Assets.Domain.UseCases;

public class VerifyAPIKeyUseCase(IMailRepository mailRepository)
{
    public async Task<bool> Execute(string apiKey)
    {
        return await mailRepository.VerifyAPI(apiKey);
    }
}