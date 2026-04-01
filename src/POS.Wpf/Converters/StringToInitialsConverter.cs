using System.Globalization;
using System.Windows.Data;

namespace POS.Wpf.Converters;

public sealed class StringToInitialsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string s || string.IsNullOrWhiteSpace(s))
            return "?";
        return s.Trim()[0].ToString().ToUpperInvariant();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
