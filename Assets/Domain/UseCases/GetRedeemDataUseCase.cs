using CathayDomain;
using CathayScraperApp.Assets.Data.Repository;

namespace CathayScraperApp.Assets.Domain.UseCases;

public class GetRedeemDataUseCase(CathayRepository repository)
{
    public async Task<CathayRedeemData?> Execute(FlightEntryToScanRequest request)
    {
        return await repository.GetRedeemData(request);
    }
}