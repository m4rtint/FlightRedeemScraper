using CathayDomain;
using CathayScraperApp.Assets.Domain.UseCases;

public class MainWindowViewModel
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly DateMatchCheckerUseCase _dateMatchCheckerUseCase;
    private readonly GetRedeemDataUseCase _getRedeemDataUseCase;
    private readonly MainWindowPresentationMapper _mainWindowPresentationMapper;
    private readonly Random _random;
    private readonly SendEmailUseCase _sendEmailUseCase;
    private readonly AddFlightRequestUseCase _addFlightUseCase;
    private readonly GetFlightsToScanUseCase _getFlightsToScanUseCase;
    private readonly DeleteFlightRequestUseCase _deleteFlightRequestUseCase;
    private MainWindowState _state;

    public MainWindowState State
    {
        get => _state;
        private set
        {
            OnStateChanged?.Invoke(value);
            _state = value;
        }
    }
    
    public event Action<MainWindowState>? OnStateChanged;

    public MainWindowViewModel(
        GetRedeemDataUseCase getRedeemDataUseCase,
        SendEmailUseCase sendEmailUseCase,
        DateMatchCheckerUseCase dateMatchCheckerUseCase,
        AddFlightRequestUseCase addFlightRequestUseCase,
        GetFlightsToScanUseCase getFlightsToScanUseCase,
        DeleteFlightRequestUseCase deleteFlightRequestUseCase,
        MainWindowPresentationMapper mainWindowPresentationMapper)
    {
        _getRedeemDataUseCase = getRedeemDataUseCase;
        _sendEmailUseCase = sendEmailUseCase;
        _dateMatchCheckerUseCase = dateMatchCheckerUseCase;
        _addFlightUseCase = addFlightRequestUseCase;
        _getFlightsToScanUseCase = getFlightsToScanUseCase;
        _deleteFlightRequestUseCase = deleteFlightRequestUseCase;
        _mainWindowPresentationMapper = mainWindowPresentationMapper;
        _cancellationTokenSource = new CancellationTokenSource();
        _random = new Random();
        State = MainWindowState.InitialState();
    }
    
    public async void StartScrape(
        CabinClass cabinClass,
        DateRange departingDateRange,
        DateRange returningDateRange,
        int repeatSeconds = 30)
    {
        DebugLogger.Log("Start Scrape");
        try
        {
            await Task.Run(() =>
            {
                PeriodicAsync(
                    async () =>
                    {
                        _getRedeemDataUseCase.Execute(cabinClass).ContinueWith(task =>
                        {
                            DebugLogger.Log("Scraping...");
                            _state = _mainWindowPresentationMapper.MapToState(task.Result);
                            State = _state;
                            SendEmail(task.Result, departingDateRange, returningDateRange);
                        });
                    },
                    TimeSpan.FromSeconds(repeatSeconds),
                    _cancellationTokenSource.Token);
            });
        }
        catch (Exception e)
        {
            DebugLogger.Log(e.Message);
        }
    }

    private async Task PeriodicAsync(Func<Task> action, TimeSpan interval, CancellationToken cancellationToken)
    {
        while (true)
        {
            await action();
            var jitter = _random.NextDouble();
            var randomMinutes = TimeSpan.FromMinutes(10 * jitter);
            DebugLogger.Log("Waiting...Extra " + randomMinutes + " Minutes");
            await Task.Delay(interval + randomMinutes, cancellationToken);
        }
    }

    public void StopScrape()
    {
        DebugLogger.Log("Stop Scrape");
        _cancellationTokenSource.Cancel();
    }
    
    public async Task AddFlightEntryRequestAsync(FlightEntryToScanRequest toScanRequest)
    {
        try
        {
            await _addFlightUseCase.Execute(toScanRequest);
            var rows = _mainWindowPresentationMapper.AppendRequest(
                previousRequests: State.FlightToScanRows,
                newRequest: toScanRequest);

            State = _state.Apply(flightsToScanRows: rows);
        }
        catch (Exception e)
        {
            DebugLogger.Log(e.Message);
        }
    }
    
    public async Task DeleteFlightEntryRequestAsync(string flightId)
    {
        try
        {
            await _deleteFlightRequestUseCase.Execute(flightId);

            var updatedRows = _mainWindowPresentationMapper.RemoveRequest(
                previousRequests: State.FlightToScanRows,
                id: flightId
            );

            State = _state.Apply(flightsToScanRows: updatedRows);
        }
        catch (Exception e)
        {
            DebugLogger.Log(e.Message);
        }
    }

    public async Task LoadStoredFlightEntryRequest()
    {
        var storedRows = await _getFlightsToScanUseCase.Execute();
        var rowStates = _mainWindowPresentationMapper.MapInitialState(storedRows);
        State = _state.Apply(flightsToScanRows: rowStates);
    }
    
    public async Task TestSendEmail(string email)
    {
        await _sendEmailUseCase.ExecuteTest(email);
    }
 
    private void SendEmail(CathayRedeemData data, DateRange departingDateRange, DateRange returningDateRange)
    {
        if (_dateMatchCheckerUseCase.Execute(
                departingDateRange,
                data.AvailabilityDestination,
                out var departureAvailable)) 
            SendEmail(departureAvailable, "|| Departing To HK");

        if (_dateMatchCheckerUseCase.Execute(
                returningDateRange,
                data.AvailabilityReturn,
            out var returnAvailable))
            SendEmail(returnAvailable, "|| Returning To YVR");
    }

    private void SendEmail(DateTime dateTime, string prefix)
    {
        var message = dateTime.ToString("MMMM dd yyyy");
        DebugLogger.Log("Found Seats and Sending Email: " + message + prefix);
        //_sendEmailUseCase.Execute("Seats available on " + message + prefix);
    }

    ~MainWindowViewModel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}

public struct MainWindowState
{
    public string? LastUpdatedTime { get; }
    public AvailabilityRowState[] AvailabilityToDestinationRows { get; }
    public AvailabilityRowState[] AvailabilityReturnRows { get; }
    public FlightToScanRowState[] FlightToScanRows { get; }

    public MainWindowState(
        string? lastUpdatedTime,
        AvailabilityRowState[] availabilityToDestinationRows,
        AvailabilityRowState[] availabilityReturnRows,
        FlightToScanRowState[] flightsToScanRows)
    {
        LastUpdatedTime = lastUpdatedTime;
        AvailabilityToDestinationRows = availabilityToDestinationRows;
        AvailabilityReturnRows = availabilityReturnRows;
        FlightToScanRows = flightsToScanRows;
    }

    public MainWindowState Apply(
        string? lastUpdatedTime = null,
        AvailabilityRowState[]? availabilityToDestinationRows = null,
        AvailabilityRowState[]? availabilityReturnRows = null,
        FlightToScanRowState[]? flightsToScanRows = null)
    {
        return new MainWindowState(
            lastUpdatedTime ?? LastUpdatedTime,
            availabilityToDestinationRows ?? AvailabilityToDestinationRows,
            availabilityReturnRows ?? AvailabilityReturnRows,
            flightsToScanRows ?? FlightToScanRows);
    }
    
    public static MainWindowState InitialState()
    {
        return new MainWindowState(
            lastUpdatedTime: null,
            availabilityToDestinationRows: Array.Empty<AvailabilityRowState>(),
            availabilityReturnRows: Array.Empty<AvailabilityRowState>(),
            flightsToScanRows: Array.Empty<FlightToScanRowState>()
        );
    }
}



public struct AvailabilityRowState
{
    public string Date;
    public string Availability;
}

public struct FlightToScanRowState
{
    public string Id { get; set; }
    public string FromAirport { get; set; }
    public string ToAirport { get; set; }
    public string CabinClass { get; set; }
    public String DepartureDate { get; set; }
    public String ReturnDate { get; set; }
    public String Email { get; set; }
}