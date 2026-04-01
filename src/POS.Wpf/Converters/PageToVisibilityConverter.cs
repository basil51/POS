using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace POS.Wpf.Converters;

/// <summary>
/// Returns Visible when value (current page string) equals ConverterParameter.
/// Used to highlight the active sidebar item.
/// </summary>
public sealed class PageToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() == parameter?.ToString() ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Returns true when value == parameter (for IsEnabled / DataTrigger use).</summary>
public sealed class PageEqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() == parameter?.ToString();

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
