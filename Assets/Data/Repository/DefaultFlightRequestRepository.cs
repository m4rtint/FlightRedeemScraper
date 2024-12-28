using CathayDomain;
using CathayScraperApp.Assets.Domain.Repository;
using CathayScraperApp.Assets.Presentation.Mappers;

namespace CathayScraperApp.Assets.Data.Repository;

public class DefaultFlightRequestRepository(FlightEntryAPI api): IFlightRequestRepository
{
    public void UpdateFlightEntries(FlightEntryToScanRequest[] entries)
    {
        // Convert domain entries to DTOs
        RedeemEntryRequestDTO[] dtoEntries = entries.Select(entry => new RedeemEntryRequestDTO
        {
            Id = entry.Id,
            FromAirport = entry.FromAirport.ToString(),
            ToAirport = entry.ToAirport.ToString(),
            Cabin = entry.Cabin.ToString(),
            DepartingOn = entry.DepartingOn,
            ReturningOn = entry.ReturningOn,
            Email = entry.Email
        }).ToArray();

        // Use the API to update flight entries
        api.UpdateFlightEntries(dtoEntries);
        DebugLogger.Log("Flight entries updated successfully.");
    }

    public async Task<FlightEntryToScanRequest[]> GetEntries()
    {
        try
        {
            // Use the API to get DTO entries
            RedeemEntryRequestDTO[] dtoEntries = await api.GetRedeemEntries();
            if (dtoEntries.Length == 0)
            {
                return [];
            }

            FlightEntryToScanRequest[] entries = dtoEntries.Select(dto => new FlightEntryToScanRequest
            {
                Id = dto.Id,
                FromAirport = AirportMapper.FromString(dto.FromAirport),
                ToAirport = AirportMapper.FromString(dto.ToAirport),
                Cabin = CabinClassMapper.MapToDomain(dto.Cabin),
                DepartingOn = dto.DepartingOn,
                ReturningOn = dto.ReturningOn,
                Email = dto.Email
            }).ToArray();

            DebugLogger.Log("Flight entries retrieved successfully.");
            return entries;
        }
        catch (Exception ex)
        {
            DebugLogger.Log($"An error occurred while retrieving flight entries: {ex.Message}");
            return Array.Empty<FlightEntryToScanRequest>();
        }
    }
}