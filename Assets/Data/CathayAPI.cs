public interface CathayAPI
{
    Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass);
}

public class CathayAPIImpl : CathayAPI
{
    public Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass)
    {
        var endpoint = new CathayRedeemEndpoint();
        var json = endpoint.Execute(origin, destination, cabinClass).Result;
        return Task.FromResult(json);
    }
}