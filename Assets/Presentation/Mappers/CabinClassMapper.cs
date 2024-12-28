using CathayDomain;

namespace CathayScraperApp.Assets.Presentation.Mappers;

public static class CabinClassMapper
{

    public static CabinClass[] GetAll()
    {
        return (CabinClass[])Enum.GetValues(typeof(CabinClass));
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
    
    public static CabinClass MapToDomain(string cabinClassString)
    {
        switch (cabinClassString)
        {
            case "Business":
                return CabinClass.Business;
            case "First":
                return CabinClass.First;
            case "PremiumEconomy":
            case "Premium Economy":
                return CabinClass.PremiumEconomy;
            case "Economy":
                return CabinClass.Economy;
            default:
                DebugLogger.Log("Unexpected cabin class string: " + cabinClassString);
                return CabinClass.Economy; // Assuming there is an Unknown value in the CabinClass enum.
        }
    }
}