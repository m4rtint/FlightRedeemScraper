using CathayDomain;

namespace CathayScraperApp.Assets.Domain.Repository;

public interface IFlightRequestRepository
{
    public void UpdateFlightEntries(FlightEntryToScanRequest[] entries);
    public Task<FlightEntryToScanRequest[]> GetEntries();
}

