using System.Net.Http;
using CathayScraperApp.Assets.Data.DTO;
using Newtonsoft.Json;

public class CathayRedeemEndpoint
{
    //https://api.cathaypacific.com/afr/search/availability/en.HKG.YVR.bus.CX.1.20240426.20241023.json
    public async Task<CathayDTO> Execute(string origin, string destination, string cabinClass)
    {
        try
        {
            var url =
                $"https://api.cathaypacific.com/afr/search/availability/en.{origin}.{destination}.{cabinClass}.CX.1.20240101.20241223.json";
            DebugLogger.Log("URL: " + url);

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(3);
                var response = await httpClient.GetAsync(url).ConfigureAwait(false)

                    ;
            
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"Error: Received HTTP {response.StatusCode} from API.";
                    DebugLogger.Log(message);
                    throw new HttpRequestException(message);
                }

                var content = await response.Content.ReadAsStringAsync();
            
                // Deserialize into CathayDTO, expecting a valid struct response
                CathayDTO cathayData;
                try
                {
                    cathayData = JsonConvert.DeserializeObject<CathayDTO>(content);
                }
                catch (JsonSerializationException ex)
                {
                    var message = "Error: Unable to deserialize the response to CathayDTO.";
                    DebugLogger.Log(message);
                    throw new JsonSerializationException(message, ex);
                }

                return cathayData;
            }
        }
        catch (HttpRequestException httpRequestEx)
        {
            // Log the request exception and re-throw it
            DebugLogger.Log("HttpRequestException: " + httpRequestEx.Message);
            throw;
        }
        catch (JsonSerializationException jsonEx)
        {
            // Log the JSON serialization exception and re-throw it
            DebugLogger.Log("JsonSerializationException: " + jsonEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            // Catch and log any other unforeseen exceptions and re-throw them
            DebugLogger.Log("Exception: " + ex.Message);
            throw;
        }
    }


}