using CathayScraperApp.Assets.Domain.Repository;

namespace CathayDomain;

public class GetFlightsToScanUseCase(IFlightRequestRepository repository)
{
    public async Task<FlightEntryToScanRequest[]> Execute()
    {
        return await repository.GetEntries();
    }
}