using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace POS.Wpf.Converters;

/// <summary>
/// Returns Visible when value is a valid, existing file path.
/// Set Inverted=true to get Visible when path is null/empty/missing (for placeholder display).
/// </summary>
public sealed class ImagePathToVisibilityConverter : IValueConverter
{
    public bool Inverted { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var hasImage = value is string s && !string.IsNullOrWhiteSpace(s) && File.Exists(s);
        var show = Inverted ? !hasImage : hasImage;
        return show ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
