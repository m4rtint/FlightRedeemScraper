using CathayScraperApp.Assets.Data.DTO;

public interface ICathayApi
{
    Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass, string fromYear, string toYear);
}

public class DefaultCathayApi : ICathayApi
{
    public Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass, string fromYear, string toYear)
    {
        var endpoint = new CathayRedeemEndpoint();
        var json = endpoint.Execute(origin, destination, cabinClass, fromYear, toYear).Result;
        return Task.FromResult(json);
    }
}