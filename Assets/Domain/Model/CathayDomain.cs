namespace CathayDomain;

public class CathayRedeemData
{
    public Availability[] AvailabilityDestination;
    public Availability[] AvailabilityReturn;
    public DateTime UpdateTime;
}

public class Availability
{
    public DateTime Date;
    public SeatsAvailability SeatsAvailability;
}

public struct FlightEntryToScanRequest
{
    public String Id;
    public Airport FromAirport;
    public Airport ToAirport;
    public CabinClass Cabin;
    public DateRange DepartingOn;
    public DateRange ReturningOn;
    public String Email;
}

public enum SeatsAvailability
{
    NotAvailable,
    HighAmount,
    LowAmount
}

public enum CabinClass
{
    First,
    Business,
    PremiumEconomy,
    Economy
}

public struct DateRange
{
    public DateTime FromDate { get; set; }  // Non-nullable DateTime for FromDate
    public DateTime? ToDate { get; set; }   // Nullable DateTime for ToDate
}
