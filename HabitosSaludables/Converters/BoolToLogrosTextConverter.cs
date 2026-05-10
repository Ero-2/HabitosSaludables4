using System.Globalization;

namespace HabitosSaludables.Converters
{
    public class BoolToLogrosTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? Color.FromArgb("#4a9eff") : Color.FromArgb("#a0a8b4");

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
