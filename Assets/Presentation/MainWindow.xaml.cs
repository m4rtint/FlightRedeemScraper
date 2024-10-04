using System.Windows;
using CathayDomain;
using CathayScraperApp.Assets.Data;
using CathayScraperApp.Assets.Data.Repository;
using CathayScraperApp.Assets.Domain.UseCases;
using CathayScraperApp.Assets.Presentation;
using CathayScraperApp.Assets.Presentation.Mappers;

namespace CathayScraperApp;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int RepeatScrapesInSeconds = 10;
    private MainWindowViewModel _viewModel;
    private readonly Polling _polling;

    public MainWindow()
    {
        InitializeComponent();
        SetupViewModel();
        SetupDebugLog();
        SetupBookingDetailEntry();
        Loaded += (sender, args) =>
        {
            _ = _viewModel?.LoadStoredFlightEntryRequest();
        };
        _polling = new Polling(intervalInSeconds: RepeatScrapesInSeconds);
    }
    
    private void SetupBookingDetailEntry()
    {
        BookingDetailEntry.OnAddFlight = HandleOnAddFlight;
        FlightDetailsDataGrid.OnDeleteFlight = HandleOnDeleteFlight;
        FlightDetailsDataGrid.OnTestSendEmail = HandleTestSendEmail;
    }

    private async void HandleOnAddFlight(FlightEntryToScanRequest flightEntryToScanRequest)
    {
        await _viewModel.AddFlightEntryRequestAsync(flightEntryToScanRequest);
    }

    private async void HandleOnDeleteFlight(string id)
    {
        await _viewModel.DeleteFlightEntryRequestAsync(id);
    }

    private async void HandleTestSendEmail(string email)
    {
        await _viewModel.TestSendEmail(email);
    }
    
    private void SetupViewModel()
    {
        var cathayAPI = new DefaultCathayApi();
        var flightEntryAPI = new FlightEntryAPI();
        var cathayRepository = new CathayRepository(cathayAPI);
        var getRedeemDataUseCase = new GetRedeemDataUseCase(cathayRepository);

        var mailRepository = new MailRepository(new MailAPI());
        var sendEmailUseCase = new SendEmailUseCase(mailRepository);
        var flightRequestRepository = new DefaultFlightRequestRepository(flightEntryAPI);
        var setFlightRequests = new AddFlightRequestUseCase(flightRequestRepository);
        var getFlightsToScan = new GetFlightsToScanUseCase(flightRequestRepository);
        var deleteFlightRequestUseCase = new DeleteFlightRequestUseCase(flightRequestRepository);
        var presentationMapper = new MainWindowPresentationMapper();
        var emailMessageBuilder = new EmailMessageBuilder();
        _viewModel = new MainWindowViewModel(
            getRedeemDataUseCase: getRedeemDataUseCase,
            sendEmailUseCase: sendEmailUseCase,
            checkAvailabilityUseCase: new CheckAvailabilityUseCase(),
            addFlightRequestUseCase: setFlightRequests,
            getFlightsToScanUseCase: getFlightsToScan,
            deleteFlightRequestUseCase: deleteFlightRequestUseCase,
            mainWindowPresentationMapper: presentationMapper,
            emailMessageBuilder: emailMessageBuilder);
        _viewModel.OnStateChanged += OnStateChanged;
    }

    private void SetupDebugLog()
    {
        DebugLogger.OnLogChanged += DebugLogger_OnLogChanged;
    }

    private void DebugLogger_OnLogChanged(string[] obj)
    {
        Dispatcher.Invoke(() => { DebugTextBox.Text = string.Join("\n", obj); });
    }

    private void OnStateChanged(MainWindowState state)
    {
        Dispatcher.Invoke(() => { RenderState(state); });
    }

    private void RenderState(MainWindowState state)
    {
        var leftResults = "";
        foreach (var item in state.AvailabilityToDestinationRows) leftResults += $"{item.Date} : {item.Availability}\n";
        LeftResults.Text = leftResults;


        var rightResults = "";
        foreach (var item in state.AvailabilityReturnRows) rightResults += $"{item.Date} : {item.Availability}\n";
        RightResults.Text = rightResults;

        FlightDetailsDataGrid.SetDetails(state.FlightToScanRows);
    }

    private void StartScrapingButtonClick(object sender, RoutedEventArgs e)
    {
        ShowInputDialog();
        /*
        _polling.StartPolling(async () =>
        {
            await _viewModel.Scrape();
        });
        */
    }
    
    private void StopScrapingButtonClick(object sender, RoutedEventArgs e)
    {
        _polling.StopPolling();
    }
    
    private void ShowInputDialog()
    {
        APIKeyVerificationWindow apiKeyWindow = new APIKeyVerificationWindow();
        apiKeyWindow.ShowDialog();
    }

}