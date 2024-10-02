using System.Windows.Controls;
using CathayDomain;

namespace CathayScraperApp;

public partial class FlightDatePicker : UserControl
{
    public FlightDatePicker()
    {
        InitializeComponent();
    }

    // Define custom events that external classes can subscribe to
    public event EventHandler<DateTime?> StartDateChanged;
    public event EventHandler<DateTime?> EndDateChanged;

    public void SetStartDate(DateTime dateTime)
    {
        StartDatePicker.SelectedDate = dateTime;
    }

    public void SetLabel(string label)
    {
        DepartingTextBlock.Text = label;
    }

    public DateRange GetDateRange()
    {
        return new DateRange
        {
            FromDate = StartDatePicker.SelectedDate ?? DateTime.Now,
            ToDate = EndDatePicker.SelectedDate == StartDatePicker.SelectedDate ? null : EndDatePicker.SelectedDate
        };
    }

    private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        StartDateChanged?.Invoke(this, StartDatePicker.SelectedDate);

        if (StartDatePicker.SelectedDate.HasValue && 
            EndDatePicker.SelectedDate.HasValue && 
            StartDatePicker.SelectedDate > EndDatePicker.SelectedDate) // Set EndDate to StartDate if StartDate is later than EndDate
            EndDatePicker.SelectedDate = StartDatePicker.SelectedDate;
    }

    private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        EndDateChanged?.Invoke(this, EndDatePicker.SelectedDate);

        if (EndDatePicker.SelectedDate.HasValue && 
            StartDatePicker.SelectedDate.HasValue && 
            EndDatePicker.SelectedDate < StartDatePicker.SelectedDate) // Set StartDate to EndDate if EndDate is earlier than StartDate
            StartDatePicker.SelectedDate = EndDatePicker.SelectedDate;
    }
}