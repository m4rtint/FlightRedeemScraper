using CathayDomain;

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

public struct CathayRedeemRequest
{
    private struct Constants
    {
        public const string FirstClass = "fir";
        public const string BusinessClass = "bus";
        public const string PremiumEconomyClass = "pey";
        public const string EconomyClass = "eco";
    }

    public readonly string CabinClass;

    public CathayRedeemRequest(CabinClass cabinClass)
    {
        CabinClass = MapToDTO(cabinClass);
    }

    private string? MapToDTO(CabinClass cabin)
    {
        switch (cabin)
        {
            case CathayDomain.CabinClass.First:
                return Constants.FirstClass;
            case CathayDomain.CabinClass.Business:
                return Constants.BusinessClass;
            case CathayDomain.CabinClass.PremiumEconomy:
                return Constants.PremiumEconomyClass;
            case CathayDomain.CabinClass.Economy:
                return Constants.EconomyClass;
        }

        return null;
    }
}