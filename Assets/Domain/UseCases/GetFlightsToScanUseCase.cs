using CathayScraperApp.Assets.Domain.Repository;

namespace CathayDomain;

public class GetFlightsToScanUseCase(IFlightRequestRepository repository)
{
    public async Task<FlightEntryToScanRequest[]> Execute()
    {
        var entries = await repository.GetEntries();
        return entries;
    }
}