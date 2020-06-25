using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Readinizer.Frontend.Converters
{
    public class ProgressForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            Brush foreground = Brushes.Green;
            Brush background = Brushes.Red;

            if (progress > 90d)
            {
                foreground = Brushes.Green;
            }
            else if (progress > 75d)
            {
                foreground = Brushes.LawnGreen;
            }
            else if (progress > 50d)
            {
                foreground = Brushes.Yellow;
            }
            else if (progress > 25d)
                {
                foreground = Brushes.Orange;
                }

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
