namespace CathayDomain
{
    public class CheckAvailabilityUseCase
    {
        public Availability[] Execute(DateRange dateRange, Availability[] availabilities)
        {
            var availableDates = new List<Availability>();

            foreach (var availability in availabilities)
            {
                // Only consider availabilities that are not marked as 'NotAvailable'
                if (availability.SeatsAvailability != SeatsAvailability.NotAvailable)
                {
                    // If ToDate is null, match only the FromDate
                    if (dateRange.ToDate == null)
                    {
                        if (availability.Date == dateRange.FromDate)
                        {
                            availableDates.Add(availability);
                        }
                    }
                    // If ToDate is set, match the range from FromDate to ToDate
                    else
                    {
                        if (availability.Date >= dateRange.FromDate && availability.Date <= dateRange.ToDate)
                        {
                            availableDates.Add(availability);
                        }
                    }
                }
            }

            // Return the list of available availabilities as an array
            return availableDates.ToArray();
        }
    }
}