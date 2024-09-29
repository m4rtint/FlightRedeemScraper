using CathayDomain;

public class GetRedeemDataUseCase
{
    private readonly CathayRepository _repository;

    public GetRedeemDataUseCase(CathayRepository repository)
    {
        _repository = repository;
    }

    public Task<CathayRedeemData?> Execute(CabinClass cabinClass)
    {
        var request = new CathayRedeemRequest(cabinClass);
        return _repository.GetRedeemData(request);
    }
}