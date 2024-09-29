using System.IO;
using System.Text.Json;

namespace CathayScraperApp.Assets.Data;

public class FlightEntryAPI
{
    private const string FileName = "FlightEntries";

    public void UpdateFlightEntries(RedeemEntryRequestDTO[] entries)
    {
        try
        {
            JsonUtility.SaveArrayToJsonFile(entries, FileName);
            DebugLogger.Log("Flight entries updated successfully.");
        }
        catch (Exception ex)
        {
            DebugLogger.Log($"An error occurred while updating flight entries: {ex.Message}");
        }
    }

    public async Task<RedeemEntryRequestDTO[]> GetRedeemEntries()
    {
        try
        {
            // Use the ReadJsonFileAsStringAsync method to read the JSON content
            var jsonString = await JsonUtility.ReadJsonFileAsStringAsync(FileName);

            // Deserialize the JSON string into an array of RedeemEntryRequestDTO
            var entries = JsonSerializer.Deserialize<RedeemEntryRequestDTO[]>(jsonString);

            DebugLogger.Log("Flight entries retrieved successfully.");
            return entries ?? [];
        }
        catch (FileNotFoundException)
        {
            DebugLogger.Log("Flight entries file does not exist.");
            return [];
        }
        catch (Exception ex)
        {
            DebugLogger.Log($"An error occurred while retrieving flight entries: {ex.Message}");
            return [];
        }
    }
}