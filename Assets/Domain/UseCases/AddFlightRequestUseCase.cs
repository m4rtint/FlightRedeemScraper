using CathayScraperApp.Assets.Domain.Repository;

namespace CathayDomain;

public class AddFlightRequestUseCase(IFlightRequestRepository repository)
{
    public async Task Execute(FlightEntryToScanRequest toScanRequest)
    {
        var entries = await repository.GetEntries();
        var updatedEntries = entries.Append(toScanRequest).ToArray();
        repository.UpdateFlightEntries(updatedEntries);
    }
}