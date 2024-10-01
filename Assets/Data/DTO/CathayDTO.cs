namespace CathayScraperApp.Assets.Data.DTO;

public struct CathayDTO
{
    public AvailabilitiesDTO availabilities;
}

public struct AvailabilitiesDTO
{
    public string updateTime;
    public AvailabilityDTO[] std;
}

public struct AvailabilityDTO
{
    public string date;
    public string availability;
}