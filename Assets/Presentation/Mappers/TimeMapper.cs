using CathayDomain;

namespace CathayScraperApp.Assets.Presentation.Mappers;

public static class TimeMapper
{
    public static Time[] GetAll()
    {
        return (Time[])Enum.GetValues(typeof(Time));
    }

    public static string MapToString(Time time)
    {
        switch (time)
        {
            case Time.Morning:
                return "Morning [12:01 AM - 11:59 AM]";
            case Time.Afternoon:
                return "Afternoon [12:00 PM - 4:59 PM]";
            case Time.Evening:
                return "Evening [5:00 PM - 11:59 PM]";
            case Time.AnyTime:
                return "Any Time";
            default:
                DebugLogger.Log("Unexpected time: " + time);
                return "Unknown"; // Add a default return value to avoid missing returns.
        }
    }

    private static string MapToModel(string time)
    {
        return time switch
        {
            "Morning [12:01 AM - 11:59 AM]" => Time.Morning.ToString(),
            "Afternoon [12:00 PM - 4:59 PM]" => Time.Afternoon.ToString(),
            "Evening [5:00 PM - 11:59 PM]" => Time.Evening.ToString(),
            "Any Time" => Time.AnyTime.ToString(),
            _ => throw new ArgumentException("Invalid Time code", nameof(time))
        };
    }
    
    public static Time FromString(string time)
    {
        var properTime = MapToModel(time);
        if (Enum.TryParse(properTime, true, out Time timeCode))
        {
            return timeCode;
        }

        throw new ArgumentException("Invalid Time code", nameof(time));
    }
}