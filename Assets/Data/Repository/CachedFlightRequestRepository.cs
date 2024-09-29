using CathayDomain;
using CathayScraperApp.Assets.Domain.Repository;

namespace CathayScraperApp.Assets.Data.Repository;

public class CachedFlightRequestRepository: IFlightRequestRepository
{
    private FlightEntryToScanRequest[] entries = Array.Empty<FlightEntryToScanRequest>(); // Initialize to an empty array
    public void UpdateFlightEntries(FlightEntryToScanRequest[] newEntries)
    {
        entries = newEntries;
    }

    public Task<FlightEntryToScanRequest[]> GetEntries()
    {
        return Task.FromResult(entries);
    }
}