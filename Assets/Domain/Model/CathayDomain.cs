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
    public Time DepartingTime;
    public Time ReturningTime;
    public String Email;
}

public struct DateRange
{
    public DateTime FromDate { get; set; }  // Non-nullable DateTime for FromDate
    public DateTime? ToDate { get; set; }   // Nullable DateTime for ToDate
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

public enum Time
{
    Morning,
    Afternoon,
    Evening,
    AnyTime
}


