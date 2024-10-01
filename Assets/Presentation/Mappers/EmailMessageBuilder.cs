using System.Text;
using CathayDomain;

namespace CathayScraperApp.Assets.Presentation.Mappers
{
    public class EmailMessageBuilder
    {
        // Function to generate the HTML email body
        public string GenerateHtmlEmail(
            string email, 
            string cabinClass,
            Availability[] departingFlights, 
            Availability[] returningFlights)
        {
            // Template string for HTML email
            var emailTemplate = $@"
            <html>
                <body>
                    <p>Dear {email},</p>

                    <p>We are pleased to provide you with the latest flight availability details for your requested itinerary. Below, you will find the available travel dates, seat availability status, and the cabin class for each date.</p>

                    <h3>Flight Availability Summary:</h3>

                    <h4>Departing Flights:</h4>
                    {GenerateHtmlFlightDetails(departingFlights, cabinClass)}

                    <h4>Returning Flights:</h4>
                    {GenerateHtmlFlightDetails(returningFlights, cabinClass)}

                    <p>Next Steps:</p>
                    <p>If you would like to proceed with booking or have any questions about this availability, please don't hesitate to contact us. We recommend booking as soon as possible, as availability may change.</p>

                    <p>Thank you for choosing us to assist with your travel plans. We look forward to helping you finalize your itinerary.</p>
                </body>
            </html>";

            return emailTemplate;
        }

        // Function to generate the plain text email body
        public string GeneratePlainTextEmail(
            string email, 
            string cabinClass,
            Availability[] departingFlights, 
            Availability[] returningFlights)
        {
            // Template string for plain text email
            var emailTemplate = $@"
Dear {email},

We are pleased to provide you with the latest flight availability details for your requested itinerary. Below, you will find the available travel dates, seat availability status, and the cabin class for each date.

Flight Availability Summary:

Departing Flights:
{GeneratePlainTextFlightDetails(departingFlights, cabinClass)}

Returning Flights:
{GeneratePlainTextFlightDetails(returningFlights, cabinClass)}

Next Steps:
If you would like to proceed with booking or have any questions about this availability, please don't hesitate to contact us. We recommend booking as soon as possible, as availability may change.

Thank you for choosing us to assist with your travel plans. We look forward to helping you finalize your itinerary.";

            return emailTemplate;
        }

        // Helper method to generate flight details for HTML
        private string GenerateHtmlFlightDetails(Availability[] flights, string cabinClass)
        {
            if (flights.Length == 0)
            {
                return "<p>No flights available.</p>";
            }

            var flightDetails = new StringBuilder();
            flightDetails.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>");
            flightDetails.AppendLine("<tr><th>Date</th><th>Cabin Class</th><th>Availability</th></tr>");

            foreach (var flight in flights)
            {
                flightDetails.AppendLine($@"
                    <tr>
                        <td>{flight.Date:MMMM dd, yyyy}</td>
                        <td>{cabinClass}</td>
                        <td>{flight.SeatsAvailability}</td>
                    </tr>");
            }

            flightDetails.AppendLine("</table>");
            return flightDetails.ToString();
        }

        // Helper method to generate flight details for plain text
        private string GeneratePlainTextFlightDetails(Availability[] flights, string cabinClass)
        {
            if (flights.Length == 0)
            {
                return "No flights available.";
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

        // Generate the email subject (same for both HTML and plain text emails)
        public string GenerateAvailabilityEmailSubject(
            Airport fromAirport, 
            Airport toAirport, 
            CabinClass cabin)
        {
            return $"Flight availability from {AirportMapper.AirportDisplayNames[fromAirport]} to {AirportMapper.AirportDisplayNames[toAirport]} in {cabin} class found";
        }
    }
}
