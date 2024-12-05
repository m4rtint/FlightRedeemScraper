using CathayDomain;

namespace CathayScraperApp.Assets.Presentation.Mappers;

public static class CabinClassMapper
{
    public static CabinClass[] GetAll()
    {
        return (CabinClass[])Enum.GetValues(typeof(CabinClass));
    }
    
    public static CabinClass FromString(string cabinClass)
    {
        if (Enum.TryParse(cabinClass, true, out CabinClass cabinClassCode))
        {
            return cabinClassCode;
        }

        throw new ArgumentException("Invalid Cabin Class code", nameof(cabinClass));
    }

    public static String MapToString(CabinClass cabinClass)
    {
        switch (cabinClass)
        {
            case CabinClass.Business:
                return "Business";
            case CabinClass.First:
                return "First";
            case CabinClass.PremiumEconomy:
                return "Premium Economy";
            case CabinClass.Economy:
                return "Economy";
            default:
                DebugLogger.Log("Unexpected cabin class: " + cabinClass);
                return "Unknown"; // Add a default return value to avoid missing returns.
        }
    }

}