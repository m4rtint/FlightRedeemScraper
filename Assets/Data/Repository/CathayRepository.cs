using CathayDomain;

public class CathayRepository(CathayAPI api)
{
    public async Task<CathayRedeemData?> GetRedeemData(CathayRedeemRequest request)
    {
        var resultsToDestination = await api.GetCathayRedeem(
            Constants.Origin,
            Constants.Destination,
            request.CabinClass);
        var resultsReturn = await api.GetCathayRedeem(
            Constants.Destination,
            Constants.Origin,
            request.CabinClass);
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

    private SeatsAvailability MapToDomain(string availability)
    {
        switch (availability)
        {
            case "NA":
                return SeatsAvailability.NotAvailable;
            case "H":
                return SeatsAvailability.HighAmount;
            case "L":
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

    private struct Constants
    {
        public const string Origin = "YVR";
        public const string Destination = "HKG";
    }
}