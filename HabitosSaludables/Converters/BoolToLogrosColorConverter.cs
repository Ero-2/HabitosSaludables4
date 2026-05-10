using System.Globalization;

namespace HabitosSaludables.Converters
{
    public class BoolToLogrosColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? Color.FromArgb("#0a1f3d") : Color.FromArgb("#e8eaed");

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
