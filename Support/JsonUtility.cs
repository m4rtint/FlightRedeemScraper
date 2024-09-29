using System.IO;
using Newtonsoft.Json;

public static class JsonUtility
{
    public static void SaveArrayToJsonFile<T>(T[] array, string fileName)
    {
        // Path to the Resources folder in the root of the project
        string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        // Check if Resources directory exists, create if not
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        // Serialize the array to JSON format
        string jsonString = JsonConvert.SerializeObject(array, Formatting.Indented);  // Using Newtonsoft.Json

        // Full path to the JSON file
        string filePath = Path.Combine(resourcesPath, fileName + ".json");

        // Write the JSON string to the file
        File.WriteAllText(filePath, jsonString);

        DebugLogger.Log($"JSON file saved to: {filePath}");
    }
    
    public static async Task<string> ReadJsonFileAsStringAsync(string fileName)
    {
        try
        {
            // Path to the Resources folder in the root of the project
            string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

            // Full path to the JSON file
            string filePath = Path.Combine(resourcesPath, fileName + ".json");
            
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the file content asynchronously
                string jsonString = await File.ReadAllTextAsync(filePath);
                return jsonString;
            }
            
            throw new FileNotFoundException($"File not found: {filePath}");
        }
        catch (Exception ex)
        {
            DebugLogger.Log($"An error occurred while reading the JSON file: {ex.Message}");
            throw;
        }
    }
}