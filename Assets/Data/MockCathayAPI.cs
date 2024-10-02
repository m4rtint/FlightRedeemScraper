using System.IO;
using CathayScraperApp.Assets.Data.DTO;
using Newtonsoft.Json;

public class MockCathayAPI : ICathayApi
{
    private const string baseDirectory = "C:\\Users\\th3in\\Documents\\CathayScraperApp";
    private const string fileName = "\\Assets\\Data\\JSON\\Cathay.json";

    public Task<CathayDTO> GetCathayRedeem(string origin, string destination, string cabinClass, string fromYear, string toYear)
    {
        var json = new CathayDTO();
        try
        {
            var fullPath = baseDirectory + fileName;
            if (File.Exists(fullPath))
            {
                var jsonString = File.ReadAllText(fullPath);
                DebugLogger.Log("Mock API Called");
                json = JsonConvert.DeserializeObject<CathayDTO>(jsonString);
            }
        }
        catch (Exception e)
        {
            DebugLogger.Log(e.Message);
        }

        return Task.FromResult(json);
    }
}