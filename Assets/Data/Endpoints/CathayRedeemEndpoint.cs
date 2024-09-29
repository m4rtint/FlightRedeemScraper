using System.Net.Http;
using Newtonsoft.Json;

public class CathayRedeemEndpoint
{
    //https://api.cathaypacific.com/afr/search/availability/en.HKG.YVR.bus.CX.1.20240426.20241023.json

    public async Task<CathayDTO> Execute(string origin, string destination, string cabinClass)
    {
        var url =
            $"https://api.cathaypacific.com/afr/search/availability/en.{origin}.{destination}.{cabinClass}.CX.1.20240101.20241223.json";
        DebugLogger.Log("URL: " + url);
        var response = await new HttpClient().GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CathayDTO>(content);
    }
}