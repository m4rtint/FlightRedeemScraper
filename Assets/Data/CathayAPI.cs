using CathayScraperApp.Assets.Data.DTO;

public interface ICathayApi
{
    Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass);
}

public class DefaultCathayApi : ICathayApi
{
    public Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass)
    {
        var endpoint = new CathayRedeemEndpoint();
        var json = endpoint.Execute(origin, destination, cabinClass).Result;
        return Task.FromResult(json);
    }
}