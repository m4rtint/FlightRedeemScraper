namespace CathayScraperApp.Assets.Domain.UseCases;

public class IsApiKeyAvailableUseCase
{
    public bool Execute()
    {
        return Environment.GetEnvironmentVariable("MailGun-ApiKey", EnvironmentVariableTarget.User) != null;
    }
}