namespace CathayDomain
{
    public class CheckAvailabilityUseCase
    {
        public Availability[] Execute(DateRange dateRange, Time timeBlock, Availability[] availabilities)
        {
            var availableDates = new List<Availability>();

            foreach (var availability in availabilities)
            {
                // Only consider availabilities that are not marked as 'NotAvailable'
                if (availability.SeatsAvailability != SeatsAvailability.NotAvailable)
                {
                    // Check if the date is within the specified range
                    bool isDateInRange = dateRange.ToDate == null
                        ? availability.Date == dateRange.FromDate
                        : availability.Date >= dateRange.FromDate && availability.Date <= dateRange.ToDate;

                    // Check if the time is within the specified time block
                    bool isTimeInBlock = IsTimeInBlock(availability.Date, timeBlock);

                    if (isDateInRange && isTimeInBlock)
                    {
                        availableDates.Add(availability);
                    }
                }
            }

            // Return the list of available availabilities as an array
            return availableDates.ToArray();
        }
        
        private bool IsTimeInBlock(DateTime dateTime, Time timeBlock)
        {
            int hour = dateTime.Hour;

            return timeBlock switch
            {
                Time.Morning => hour >= 0 && hour < 12,
                Time.Afternoon => hour >= 12 && hour < 17,
                Time.Evening => hour >= 17 && hour < 24,
                Time.AnyTime => true,
                _ => false,
            };
        }
    }
}