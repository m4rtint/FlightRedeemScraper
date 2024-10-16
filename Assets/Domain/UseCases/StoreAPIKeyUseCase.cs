namespace CathayScraperApp.Assets.Domain.UseCases;

public class StoreApiKeyUseCase
{
    public void Execute(string apiKey)
    {
        Environment.SetEnvironmentVariable("MailGun-ApiKey", apiKey, EnvironmentVariableTarget.User);
    }
}