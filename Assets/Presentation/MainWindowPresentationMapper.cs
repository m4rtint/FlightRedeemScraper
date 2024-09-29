using CathayDomain;

public class MainWindowPresentationMapper
{
    public MainWindowState MapToState(CathayRedeemData data)
    {
        return new MainWindowState(
            lastUpdatedTime: data.UpdateTime.ToString("yyyy-MM-dd"),
            availabilityToDestinationRows: MapToRows(data.AvailabilityDestination),
            availabilityReturnRows: MapToRows(data.AvailabilityReturn),
            flightsToScanRows: []
        );
    }
    
    private AvailabilityRowState[] MapToRows(Availability[] availabilities)
    {
        var rowStates = new AvailabilityRowState[availabilities.Length];
        for (var i = 0; i < availabilities.Length; i++)
            rowStates[i] = new AvailabilityRowState
            {
                Date = availabilities[i].Date.ToString("yyyy-MM-dd"),
                Availability = availabilities[i].SeatsAvailability.ToString()
            };

        return rowStates;
    }

    public FlightToScanRowState[] AppendRequest(
        FlightToScanRowState[] previousRequests,
        FlightEntryToScanRequest newRequest)
    {
        var mappedPreviousRequests = previousRequests.ToList();
        mappedPreviousRequests.Add(MapToRowState(newRequest));
        return mappedPreviousRequests.ToArray();
    }
    
    public FlightToScanRowState[] RemoveRequest(
        FlightToScanRowState[] previousRequests,
        string id)
    {
        return previousRequests
            .Where(request => request.Id != id) 
            .ToArray();  
    }

    public FlightToScanRowState[] MapInitialState(FlightEntryToScanRequest[] requests)
    {
        if (requests.Length == 0)
        {
            return [];
        }

        return requests.Select(MapToRowState).ToArray();
    }

    private FlightToScanRowState MapToRowState(FlightEntryToScanRequest request)
    {
        return new FlightToScanRowState()
        {
            Id = request.Id,
            FromAirport =
                AirportMapper.AirportDisplayNames
                    [request.FromAirport],
            ToAirport = AirportMapper.AirportDisplayNames[request.ToAirport],
            CabinClass = request.Cabin.ToString(),
            DepartureDate = MapToDateRange(request.DepartingOn),
            ReturnDate = MapToDateRange(request.ReturningOn),
            Email = request.Email
        };
    }

    private string MapToDateRange(DateRange dateRange)
    {
        string result = $"{dateRange.FromDate:dd/MM/yyyy}";
        if (dateRange.ToDate.HasValue)
        {
            result += $" - {dateRange.ToDate:dd/MM/yyyy}";
        }

        return result;
    }

}