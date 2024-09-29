using System.Globalization;
using System.Windows.Data;
using CathayDomain;

public class AirportToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Airport airport && AirportMapper.AirportDisplayNames.TryGetValue(airport, out var displayName))
            return displayName;
        return value?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        foreach (var kvp in AirportMapper.AirportDisplayNames)
            if (kvp.Value == value.ToString())
                return kvp.Key;
        throw new InvalidOperationException("Unable to convert back");
    }
}