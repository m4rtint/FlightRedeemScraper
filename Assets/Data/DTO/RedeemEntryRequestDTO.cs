using CathayDomain;

namespace CathayScraperApp.Assets.Data;

public class RedeemEntryRequestDTO
{
    public string Id { get; set; }
    public string FromAirport { get; set; }
    public string ToAirport { get; set; }
    public string Cabin { get; set; }
    public DateRange DepartingOn { get; set; }
    public DateRange ReturningOn { get; set; }
    public string DepartingTime { get; set; }
    public string ReturningTime { get; set; }
    public string Email { get; set; }
}