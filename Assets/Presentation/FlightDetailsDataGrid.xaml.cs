using System.Windows;
using System.Windows.Controls;

namespace CathayScraperApp.Assets.Presentation;

public partial class FlightDetailsDataGrid : UserControl
{
    public Action<string> OnDeleteFlight;
    public Action<string> OnTestSendEmail;
    
    public FlightDetailsDataGrid()
    {
        InitializeComponent();
    }

    public void SetDetails(FlightToScanRowState[] flights)
    {
        DetailsDataGrid.ItemsSource = flights;
    }

    private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: FlightToScanRowState flight })
        {
            OnDeleteFlight?.Invoke(flight.Id);
        }
    }

    private void TestEmailButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: FlightToScanRowState flight })
        {
            OnTestSendEmail?.Invoke(flight.Email);
        }
    }
}