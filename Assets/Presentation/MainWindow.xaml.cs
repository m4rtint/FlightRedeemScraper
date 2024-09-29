using System.Windows;
using CathayDomain;
using CathayScraperApp.Assets.Data;
using CathayScraperApp.Assets.Data.Repository;

namespace CathayScraperApp;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int RepeatScrapeSeconds = 60 * 15;
    private MainWindowViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        SetupViewModel();
        SetupDebugLog();
        SetupBookingDetailEntry();
        Loaded += (sender, args) =>
        {
            _ = viewModel?.LoadStoredFlightEntryRequest();
        };
    }
    
    private void SetupBookingDetailEntry()
    {
        BookingDetailEntry.OnAddFlight = HandleOnAddFlight;
        FlightDetailsDataGrid.OnDeleteFlight = HandleOnDeleteFlight;
    }

    private async void HandleOnAddFlight(FlightEntryToScanRequest flightEntryToScanRequest)
    {
        await viewModel.AddFlightEntryRequestAsync(flightEntryToScanRequest);
    }

    private async void HandleOnDeleteFlight(string id)
    {
        await viewModel.DeleteFlightEntryRequestAsync(id);
    }
    
    private void SetupViewModel()
    {
        var cathayAPI = new CathayAPIImpl();
        var flightEntryAPI = new FlightEntryAPI();
        var cathayRepository = new CathayRepository(cathayAPI);
        var getRedeemDataUseCase = new GetRedeemDataUseCase(cathayRepository);

        var rateLimitEmail = new RateLimitEmail(5, 1, new SimpleStorage());
        var mailRepository = new MailRepository(new MailAPI());
        var sendEmailUseCase = new SendEmailUseCase(mailRepository, rateLimitEmail);
        var flightRequestRepository = new DefaultFlightRequestRepository(flightEntryAPI);
        var setFlightRequests = new AddFlightRequestUseCase(flightRequestRepository);
        var getFlightsToScan = new GetFlightsToScanUseCase(flightRequestRepository);
        var deleteFlightRequestUseCase = new DeleteFlightRequestUseCase(flightRequestRepository);
        var presentationMapper = new MainWindowPresentationMapper();
        viewModel = new MainWindowViewModel(
            getRedeemDataUseCase: getRedeemDataUseCase,
            sendEmailUseCase: sendEmailUseCase,
            dateMatchCheckerUseCase: new DateMatchCheckerUseCase(),
            addFlightRequestUseCase: setFlightRequests,
            getFlightsToScanUseCase: getFlightsToScan,
            deleteFlightRequestUseCase: deleteFlightRequestUseCase,
            mainWindowPresentationMapper: presentationMapper);
        viewModel.OnStateChanged += OnStateChanged;
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
        Dispatcher.Invoke(() => { UpdateUI(state); });
    }

    private void UpdateUI(MainWindowState state)
    {
        var leftResults = "";
        foreach (var item in state.AvailabilityToDestinationRows) leftResults += $"{item.Date} : {item.Availability}\n";
        LeftResults.Text = leftResults;


        var rightResults = "";
        foreach (var item in state.AvailabilityReturnRows) rightResults += $"{item.Date} : {item.Availability}\n";
        RightResults.Text = rightResults;

        FlightDetailsDataGrid.SetDetails(state.FlightToScanRows);
    }

    private void LeftButtonClick(object sender, RoutedEventArgs e)
    {
        /*
        CabinClassPicker.IsEnabled = false;
        DepartingDatePicker.IsEnabled = false;
        ReturningDatePicker.IsEnabled = false;
        LeftButton.IsEnabled = false;
        _viewModel.StartScrape(
            cabinClass: GetCabinClass(),
            repeatSeconds: RepeatScrapeSeconds,
            departingDateRange: DepartingDatePicker.GetDateRange(),
            returningDateRange: ReturningDatePicker.GetDateRange());
            */
    }


    /*
     * Change combo box into a list of cabin classes you can tick
     */
    private void RightButtonClick(object sender, RoutedEventArgs e)
    {
        //CabinClassPicker.IsEnabled = true;
        // DepartingDatePicker.IsEnabled = true;
        //ReturningDatePicker.IsEnabled = true;
        LeftButton.IsEnabled = true;
        viewModel.StopScrape();
    }
}