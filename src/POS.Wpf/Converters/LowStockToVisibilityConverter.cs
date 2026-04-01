using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace POS.Wpf.Converters;

/// <summary>
/// Returns Visible when a decimal stock value is at or below the threshold.
/// Default threshold: 5. Set Threshold via ConverterParameter (string) or property.
/// </summary>
public sealed class LowStockToVisibilityConverter : IValueConverter
{
    public decimal Threshold { get; set; } = 5m;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var threshold = Threshold;
        if (parameter is string s && decimal.TryParse(s, out var pt))
            threshold = pt;

        if (value is decimal d)
            return d <= threshold ? Visibility.Visible : Visibility.Collapsed;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
