using CathayScraperApp.Assets.Domain.Repository;

namespace CathayDomain;

public class DeleteFlightRequestUseCase(IFlightRequestRepository repository)
{
    public async Task Execute(string flightId)
    {
        var entries = await repository.GetEntries();
        var updatedEntries = entries.Where(entry => entry.Id != flightId).ToArray();
        repository.UpdateFlightEntries(updatedEntries);
    }
}