using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1.Converters
{
    public class BrightnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush && parameter is string factor)
            {
                if (double.TryParse(factor, out double brightnessFactor))
                {
                    Color color = brush.Color;
                    byte r = (byte)Math.Min(255, color.R * brightnessFactor);
                    byte g = (byte)Math.Min(255, color.G * brightnessFactor);
                    byte b = (byte)Math.Min(255, color.B * brightnessFactor);
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
