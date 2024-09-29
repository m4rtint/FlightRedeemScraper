using System.Windows;
using System.Windows.Controls;
using CathayDomain;
using CathayScraperApp.Assets.Presentation.Mappers;

namespace CathayScraperApp.Assets.Presentation;

public partial class BookingDetailEntry : UserControl
{
    public Action<FlightEntryToScanRequest> OnAddFlight;
    private struct Constants
    {
        public const string DepartingDatePickerTitle = "Departing On";
        public const string ReturningDatePickerTitle = "Returning On";
    }
    
    public BookingDetailEntry()
    {
        InitializeComponent();
        SetupDatePicker();
        SetupCabinPicker();
    }

    private void ReturningDatePicker_StartDateChanged(object sender, DateTime? newDate)
    {
        UpdateUIOnDateChanged();
    }

    private void DepartingDatePicker_StartDateChanged(object? sender, DateTime? e)
    {
        UpdateUIOnDateChanged();
    }
    
    private void SetupDatePicker()
    {
        DepartingDatePicker.SetLabel(Constants.DepartingDatePickerTitle);
        DepartingDatePicker.SetStartDate(DateTime.Today);

        ReturningDatePicker.SetLabel(Constants.ReturningDatePickerTitle);
        ReturningDatePicker.SetStartDate(DateTime.Today.AddDays(10));
    }

    private void SetupCabinPicker()
    {
        foreach (var cabinClass in CabinClassMapper.GetAll())
        {
            CabinClassPicker.Items.Add(new ComboBoxItem
            {
                Content = CabinClassMapper.MapToString(cabinClass)
            });
        }

        SetCabin(CabinClass.Business);
    }

    private void SetCabin(CabinClass cabinClass)
    {
        foreach (var item in CabinClassPicker.Items)
        {
            if (item is ComboBoxItem comboBoxItem && 
                comboBoxItem.Content.ToString() == CabinClassMapper.MapToString(cabinClass))
            {
                CabinClassPicker.SelectedItem = comboBoxItem;
                break;
            }
        }
    }
    
    private CabinClass GetCabinClassFromPicker()
    {
        if (CabinClassPicker.SelectedItem is ComboBoxItem selectedItem)
        {
            string? selectedContent = selectedItem.Content.ToString();
            if (selectedContent != null)
            {
                return CabinClassMapper.FromString(selectedContent);
            }
            DebugLogger.Log("No cabin class selected or incorrect item type.");
        }
        else
        {
            DebugLogger.Log("No cabin class selected or incorrect item type.");
        }

        return CabinClass.Economy;
    }

    private void UpdateUIOnDateChanged()
    {
        var departingRange = DepartingDatePicker.GetDateRange();
        var returningRange = ReturningDatePicker.GetDateRange();

        if (returningRange.FromDate < departingRange.FromDate || returningRange.FromDate < departingRange.ToDate)
        {
            ErrorPanel.Visibility = Visibility.Visible;
        }
        ErrorPanel.Visibility = Visibility.Collapsed;
    }

    private void AddFlightButtonClick(object sender, RoutedEventArgs e)
    {
        var entry = new FlightEntryToScanRequest()
        {
            Id = Guid.NewGuid().ToString(),
            FromAirport = TravelLocationPicker.SelectedFromAirport,
            ToAirport = TravelLocationPicker.SelectedToAirport,
            Cabin = GetCabinClassFromPicker(),
            DepartingOn = DepartingDatePicker.GetDateRange(),
            ReturningOn = ReturningDatePicker.GetDateRange(),
            Email = EmailInput.Text,
        };
        
        OnAddFlight?.Invoke(entry);
    }
}