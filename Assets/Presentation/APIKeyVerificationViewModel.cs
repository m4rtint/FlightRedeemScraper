using CathayScraperApp.Assets.Domain.UseCases;

namespace CathayScraperApp.Assets.Presentation
{
    public class APIKeyVerificationViewModel(
        VerifyAPIKeyUseCase verifyAPIKeyUseCase, 
        StoreApiKeyUseCase storeApiKeyUseCase)
    {
        public async Task<bool> Verify(string api)
        {
            bool isVerified = await verifyAPIKeyUseCase.Execute(api);
            return isVerified;
        }
        
        public void Store(string api)
        {
            storeApiKeyUseCase.Execute(api);
        }
    }
}