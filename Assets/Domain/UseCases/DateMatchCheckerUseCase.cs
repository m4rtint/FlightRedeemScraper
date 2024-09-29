namespace CathayDomain;

public class DateMatchCheckerUseCase
{
    public bool Execute(DateRange dateRange, Availability[] availabilities, out DateTime dateAvailable)
    {
        foreach (var availability in availabilities)
            if (dateRange.ToDate == null)
            {
                if (availability.Date == dateRange.FromDate)
                {
                    dateAvailable = availability.Date;
                    return true;
                }
            }
            else
            {
                if (availability.Date >= dateRange.FromDate && availability.Date <= dateRange.ToDate)
                {
                    dateAvailable = availability.Date;
                    return true;
                }
            }

        dateAvailable = default;
        return false;
    }
}