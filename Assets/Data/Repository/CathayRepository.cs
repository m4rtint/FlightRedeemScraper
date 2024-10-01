using CathayDomain;
using CathayScraperApp.Assets.Data.DTO;

namespace CathayScraperApp.Assets.Data.Repository;

public class CathayRepository(ICathayApi api)
{
    private struct Constants
    {
        public const string FirstClass = "fir";
        public const string BusinessClass = "bus";
        public const string PremiumEconomyClass = "pey";
        public const string EconomyClass = "eco";
        public const string NotAvailable = "NA";
        public const string HighAmount = "H";
        public const string LowAmount = "L";
    }
    
    public async Task<CathayRedeemData?> GetRedeemData(FlightEntryToScanRequest request)
    {
        var origin = request.FromAirport.ToString();
        var destination = request.ToAirport.ToString();
        var cabin = MapToDTO(request.Cabin);
        
        var resultsToDestination = await api.GetCathayRedeem(
            origin,
            destination,
            cabin);
        var resultsReturn = await api.GetCathayRedeem(
            destination,
            origin,
            cabin);
        return MapToDomain(resultsToDestination.availabilities, resultsReturn.availabilities);
    }

    private CathayRedeemData? MapToDomain(AvailabilitiesDTO dtoToDestination, AvailabilitiesDTO dtoReturn)
    {
        if (!DateTime.TryParse(dtoToDestination.updateTime, out var dateTime)) return null;

        Availability[] availabilitiesToDestination = dtoToDestination.std
            .Select(MapToDomain)
            .Where(x => x != null)
            .ToArray()!;
        Availability[] availabilitiesReturn = dtoReturn.std
            .Select(MapToDomain)
            .Where(x => x != null)
            .ToArray()!;

        CathayRedeemData cathayRedeemData = new();
        cathayRedeemData.UpdateTime = dateTime;
        cathayRedeemData.AvailabilityDestination = availabilitiesToDestination;
        cathayRedeemData.AvailabilityReturn = availabilitiesReturn;

        return cathayRedeemData;
    }
    
    private string MapToDTO(CabinClass cabin)
    {
        switch (cabin)
        {
            case CabinClass.First:
                return Constants.FirstClass;
            case CabinClass.Business:
                return Constants.BusinessClass;
            case CabinClass.PremiumEconomy:
                return Constants.PremiumEconomyClass;
            case CabinClass.Economy:
                return Constants.EconomyClass;
            default:
                DebugLogger.Log("Invalid Cabin Class: " + cabin);
                throw new ArgumentOutOfRangeException(nameof(cabin), "Invalid or missing cabin class");
        }
    }


    private SeatsAvailability MapToDomain(string availability)
    {
        switch (availability)
        {
            case Constants.NotAvailable:
                return SeatsAvailability.NotAvailable;
            case Constants.HighAmount:
                return SeatsAvailability.HighAmount;
            case Constants.LowAmount:
                return SeatsAvailability.LowAmount;
            default:
                return SeatsAvailability.NotAvailable;
        }
    }

    private DateTime MapToDomain(int number)
    {
        // Extract year, month, and day from the number
        var year = number / 10000;
        var month = number / 100 % 100;
        var day = number % 100;

        // Create a DateTime object
        return new DateTime(year, month, day);
    }

    private Availability? MapToDomain(AvailabilityDTO dto)
    {
        if (!int.TryParse(dto.date, out var date)) return null;

        var data = new Availability();
        data.Date = MapToDomain(date);
        data.SeatsAvailability = MapToDomain(dto.availability);

        return data.SeatsAvailability == SeatsAvailability.NotAvailable ? null : data;
    }
}