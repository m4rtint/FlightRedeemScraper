using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CathayDomain;

namespace CathayScraperApp.Assets.Presentation;

public partial class TravelLocationPicker : UserControl
{
    private readonly AirportToStringConverter _airportToStringConverter = new();

    public Airport SelectedFromAirport => (Airport)FromDestinationPicker.SelectedItem;
    public Airport SelectedToAirport => (Airport)ToDestinationPicker.SelectedItem;
    
    public TravelLocationPicker()
    {
        InitializeComponent();
        PopulateComboBoxes();
    }

    private void PopulateComboBoxes()
    {
        var airports = Enum.GetValues(typeof(Airport)).Cast<Airport>().ToList();

        // Sort the airports alphabetically based on their display names
        airports = airports.OrderBy(a => AirportMapper.AirportDisplayNames[a]).ToList();

        FromDestinationPicker.ItemsSource = airports;
        ToDestinationPicker.ItemsSource = airports;

        FromDestinationPicker.ItemTemplate = CreateDataTemplate();
        ToDestinationPicker.ItemTemplate = CreateDataTemplate();
        
        FromDestinationPicker.SelectedItem = airports.FirstOrDefault(a => a == Airport.YVR);
        ToDestinationPicker.SelectedItem = airports.FirstOrDefault(a => a == Airport.HKG);
    }

    private DataTemplate CreateDataTemplate()
    {
        var dataTemplate = new DataTemplate();
        var factory = new FrameworkElementFactory(typeof(TextBlock));
        factory.SetBinding(TextBlock.TextProperty, new Binding
        {
            Converter = _airportToStringConverter
        });
        dataTemplate.VisualTree = factory;
        return dataTemplate;
    }
}