using System.Globalization;

namespace HabitosSaludables.Converters
{
    public class BoolToLogrosBorderConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? Color.FromArgb("#2e6abf") : Color.FromArgb("#d0d4da");

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
