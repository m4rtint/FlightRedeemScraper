using CathayDomain;
using CathayScraperApp.Assets.Domain.UseCases;
using CathayScraperApp.Assets.Presentation.Mappers;

namespace CathayScraperApp.Assets.Presentation;

public class MainWindowViewModel
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CheckAvailabilityUseCase _checkAvailabilityUseCase;
    private readonly GetRedeemDataUseCase _getRedeemDataUseCase;
    private readonly MainWindowPresentationMapper _mainWindowPresentationMapper;
    private readonly Random _random;
    private readonly SendEmailUseCase _sendEmailUseCase;
    private readonly AddFlightRequestUseCase _addFlightUseCase;
    private readonly GetFlightsToScanUseCase _getFlightsToScanUseCase;
    private readonly DeleteFlightRequestUseCase _deleteFlightRequestUseCase;
    private readonly EmailMessageBuilder _emailMessageBuilder;
    private MainWindowState _state;

    private MainWindowState State
    {
        get => _state;
        set
        {
            OnStateChanged?.Invoke(value);
            _state = value;
        }
    }
    
    public event Action<MainWindowState>? OnStateChanged;

    public MainWindowViewModel(
        GetRedeemDataUseCase getRedeemDataUseCase,
        SendEmailUseCase sendEmailUseCase,
        CheckAvailabilityUseCase checkAvailabilityUseCase,
        AddFlightRequestUseCase addFlightRequestUseCase,
        GetFlightsToScanUseCase getFlightsToScanUseCase,
        DeleteFlightRequestUseCase deleteFlightRequestUseCase,
        MainWindowPresentationMapper mainWindowPresentationMapper,
        EmailMessageBuilder emailMessageBuilder)
    {
        _getRedeemDataUseCase = getRedeemDataUseCase;
        _sendEmailUseCase = sendEmailUseCase;
        _checkAvailabilityUseCase = checkAvailabilityUseCase;
        _addFlightUseCase = addFlightRequestUseCase;
        _getFlightsToScanUseCase = getFlightsToScanUseCase;
        _deleteFlightRequestUseCase = deleteFlightRequestUseCase;
        _mainWindowPresentationMapper = mainWindowPresentationMapper;
        _emailMessageBuilder = emailMessageBuilder;
        _cancellationTokenSource = new CancellationTokenSource();
        _random = new Random();
        State = MainWindowState.InitialState();
    }

    public async Task Scrape()
    {
        DebugLogger.Log("Scraping...");
        var flightEntryRequests = await _getFlightsToScanUseCase.Execute();
        foreach (var request in flightEntryRequests)
        {
            try
            {
                var data = await _getRedeemDataUseCase.Execute(request);
                if (data != null)
                {
                    await SendEmail(data, request);
                }
                else
                {
                    DebugLogger.Log("Cathay Fetch Data returned Null");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Log($"Error processing request {request}: {ex.Message}");
            }
        }
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
 
    private async Task SendEmail(CathayRedeemData data, FlightEntryToScanRequest request)
    {
        var departures = _checkAvailabilityUseCase.Execute(request.DepartingOn, data.AvailabilityDestination);
        var returns = _checkAvailabilityUseCase.Execute(request.ReturningOn, data.AvailabilityReturn);
        if (departures.Length > 0 || returns.Length > 0)
        {
            var message = _emailMessageBuilder.GeneratePlainTextEmail(
                email: request.Email, 
                cabinClass: request.Cabin.ToString(),
                departingFlights: departures, 
                returningFlights: returns);
            var htmlMessage = _emailMessageBuilder.GenerateHtmlEmail(
                email: request.Email, 
                cabinClass: request.Cabin.ToString(),
                departingFlights: departures, 
                returningFlights: returns);
            var subject = _emailMessageBuilder.GenerateAvailabilityEmailSubject(request.FromAirport, request.ToAirport, request.Cabin);
            DebugLogger.Log("Found Seats and Sending Email: " + request.Email);
            await _sendEmailUseCase.Execute(request.Email, subject, message, htmlMessage);
        }
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
    public FlightEntryToScanRequest[] ScanRequests { get; }

    public MainWindowState(
        string? lastUpdatedTime,
        AvailabilityRowState[] availabilityToDestinationRows,
        AvailabilityRowState[] availabilityReturnRows,
        FlightToScanRowState[] flightsToScanRows, 
        FlightEntryToScanRequest[] scanRequests)
    {
        LastUpdatedTime = lastUpdatedTime;
        AvailabilityToDestinationRows = availabilityToDestinationRows;
        AvailabilityReturnRows = availabilityReturnRows;
        FlightToScanRows = flightsToScanRows;
        ScanRequests = scanRequests;
    }

    public MainWindowState Apply(
        string? lastUpdatedTime = null,
        AvailabilityRowState[]? availabilityToDestinationRows = null,
        AvailabilityRowState[]? availabilityReturnRows = null,
        FlightToScanRowState[]? flightsToScanRows = null,
        FlightEntryToScanRequest[]? scanRequests = null)
    {
        return new MainWindowState(
            lastUpdatedTime ?? LastUpdatedTime,
            availabilityToDestinationRows ?? AvailabilityToDestinationRows,
            availabilityReturnRows ?? AvailabilityReturnRows,
            flightsToScanRows ?? FlightToScanRows, 
            scanRequests ?? ScanRequests);
    }
    
    public static MainWindowState InitialState()
    {
        return new MainWindowState(
            lastUpdatedTime: null,
            availabilityToDestinationRows: [],
            availabilityReturnRows: [],
            flightsToScanRows: [],
            scanRequests: []
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