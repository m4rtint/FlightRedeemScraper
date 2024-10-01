using System.Text;
using CathayDomain;

namespace CathayScraperApp.Assets.Presentation.Mappers;

public class EmailMessageBuilder
{
    public string GenerateAvailabilityEmail(
        string email, 
        string cabinClass,
        Availability[] departingFlights, 
        Availability[] returningFlights)
    {
        // Template string for email
        var emailTemplate = $@"
Dear {email},

We are pleased to provide you with the latest flight availability details for your requested itinerary. Below, you will find the available travel dates, seat availability status, and the cabin class for each date.

Flight Availability Summary:

Departing Flights:
{GenerateFlightDetails(departingFlights, cabinClass)}

Returning Flights:
{GenerateFlightDetails(returningFlights, cabinClass)}

Next Steps:
If you would like to proceed with booking or have any questions about this availability, please don't hesitate to contact us. We recommend booking as soon as possible, as availability may change.

Thank you for choosing us to assist with your travel plans. We look forward to helping you finalize your itinerary.
";

        return emailTemplate;
    }

// Helper method to format flight details
    private string GenerateFlightDetails(Availability[] flights, string cabinClass)
    {
        if (flights.Length == 0)
        {
            return "No flights available.\n";
        }

        var flightDetails = new StringBuilder();
        flightDetails.AppendLine("| Date             | Cabin Class    | Availability      |");
        flightDetails.AppendLine("|------------------|----------------|-------------------|");

        foreach (var flight in flights)
        {
            flightDetails.AppendLine($"| {flight.Date:MMMM dd, yyyy} | {cabinClass}      | {flight.SeatsAvailability}       |");
        }

        return flightDetails.ToString();
    }

    public string GenerateAvailabilityEmailSubject(
        Airport fromAirport, 
        Airport toAirport, 
        CabinClass cabin)
    {
        return $"Flight availability from {AirportMapper.AirportDisplayNames[fromAirport]} to {AirportMapper.AirportDisplayNames[toAirport]} in {cabin} class found";
    }

}