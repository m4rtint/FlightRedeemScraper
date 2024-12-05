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
        public const string DepartingDatePickerTitle = "Departure Date";
        public const string ReturningDatePickerTitle = "Return Date";
        public const string AirportChosenErrorMessage = "Departure and arrival airports must be different.";
        public const string EmailErrorMessage = "Please enter a valid email address";
        public const string DatePickerErrorMessage =
            "Returning date must be after the departing date and within the departing date range.";
    }
    
    public BookingDetailEntry()
    {
        InitializeComponent();
        SetupDatePicker();
        SetupCabinPicker();
        SetupTimePicker();
        TravelLocationPicker.TravelLocationChanged += TravelLocationPickerOnTravelLocationChanged;
    }

    private void TravelLocationPickerOnTravelLocationChanged(object? sender, EventArgs e)
    {
        var fromSelected = TravelLocationPicker.FromDestinationPicker.SelectedItem;
        var toSelected = TravelLocationPicker.ToDestinationPicker.SelectedItem;

        if (fromSelected.Equals(toSelected))
        {
            ShowErrorMessage(Constants.AirportChosenErrorMessage);
        }
        else
        {
            HideErrorMessage();
        }
    }

    private void ReturningDatePicker_StartDateChanged(object? sender, DateTime? newDate)
    {
        CheckForIncorrectDates();
    }

    private void DepartingDatePicker_StartDateChanged(object? sender, DateTime? e)
    {
        CheckForIncorrectDates();
    }
    
    private void DepartingDatePicker_EndDateChanged(object? sender, DateTime? e)
    {
        CheckForIncorrectDates();
    }
    
    private void ReturningDatePicker_EndDateChanged(object? sender, DateTime? e)
    {
        CheckForIncorrectDates();
    }
    
    private void SetupDatePicker()
    {
        DepartingDatePicker.SetLabel(Constants.DepartingDatePickerTitle);
        DepartingDatePicker.SetStartDate(DateTime.Today);

        ReturningDatePicker.SetLabel(Constants.ReturningDatePickerTitle);
        ReturningDatePicker.SetStartDate(DateTime.Today.AddDays(10));
        
        DepartingDatePicker.StartDateChanged += DepartingDatePicker_StartDateChanged;
        DepartingDatePicker.EndDateChanged += DepartingDatePicker_EndDateChanged;
    
        ReturningDatePicker.StartDateChanged += ReturningDatePicker_StartDateChanged;
        ReturningDatePicker.EndDateChanged += ReturningDatePicker_EndDateChanged;
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
    
    private void SetupTimePicker()
    {
        foreach(var time in TimeMapper.GetAll())
        {
            ReturnTimePicker.Items.Add(new ComboBoxItem
            {
                Content = TimeMapper.MapToString(time)
            });
            
            DepartureTimePicker.Items.Add(new ComboBoxItem
            {
                Content = TimeMapper.MapToString(time)
            });
        }
        
        SetTime(Time.AnyTime, ReturnTimePicker);
        SetTime(Time.AnyTime, DepartureTimePicker);
    }
    
    private void SetTime(Time time, ComboBox comboBox)
    {
        foreach (var item in comboBox.Items)
        {
            if (item is ComboBoxItem comboBoxItem && 
                comboBoxItem.Content.ToString() == TimeMapper.MapToString(time))
            {
                comboBox.SelectedItem = comboBoxItem;
                break;
            }
        }
    }
    
    private Time GetTimeFromPicker(ComboBox comboBox)
    {
        if (comboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string? selectedContent = selectedItem.Content.ToString();
            if (selectedContent != null)
            {
                return TimeMapper.FromString(selectedContent);
            }
            DebugLogger.Log("No time selected or incorrect item type.");
        }
        else
        {
            DebugLogger.Log("No time selected or incorrect item type.");
        }

        return Time.AnyTime;
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

    private void CheckForIncorrectDates()
    {
        var departingRange = DepartingDatePicker.GetDateRange();
        var returningRange = ReturningDatePicker.GetDateRange();
        
        if (returningRange.FromDate <= departingRange.FromDate ||
            (departingRange.ToDate.HasValue && returningRange.FromDate <= departingRange.ToDate.Value))
        {
            ShowErrorMessage(Constants.DatePickerErrorMessage);
        }
        else
        {
            HideErrorMessage();
        }
    }

    private void AddFlightButtonClick(object sender, RoutedEventArgs e)
    {
        if (EmailChecker.IsValidEmail(EmailInput.Text))
        {
            var entry = new FlightEntryToScanRequest()
            {
                Id = Guid.NewGuid().ToString(),
                FromAirport = TravelLocationPicker.SelectedFromAirport,
                ToAirport = TravelLocationPicker.SelectedToAirport,
                Cabin = GetCabinClassFromPicker(),
                DepartingOn = DepartingDatePicker.GetDateRange(),
                ReturningOn = ReturningDatePicker.GetDateRange(),
                DepartingTime = GetTimeFromPicker(DepartureTimePicker),
                ReturningTime = GetTimeFromPicker(ReturnTimePicker),
                Email = EmailInput.Text,
            };

            EmailInput.Clear();
            OnAddFlight?.Invoke(entry);
        }
        else
        {
            ShowErrorMessage(Constants.EmailErrorMessage);
        }
    }

    private void EmailInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        var email = (sender as TextBox)?.Text;
        if (email != null && EmailChecker.IsValidEmail(email) && email.Length > 0)
        {
            HideErrorMessage();
        }
    }

    private void ShowErrorMessage(string message)
    {
        ErrorPanel.Visibility = Visibility.Visible;
        ErrorMessage.Text = message;
        AddFlightButton.IsEnabled = false;
    }

    private void HideErrorMessage()
    {
        ErrorPanel.Visibility = Visibility.Collapsed;
        AddFlightButton.IsEnabled = true;
    }
}