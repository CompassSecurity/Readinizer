using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Frontend.Converters
{
    public class NullToInvisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case ObservableCollection<ADDomain> observableCollection1:
                {
                    return observableCollection1.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
                }
                case ObservableCollection<OrganizationalUnit> organisationalUnits:
                {
                    var observableCollection = organisationalUnits;
                    return observableCollection.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
                }
                default:
                {
                    return value == null ? Visibility.Hidden : Visibility.Visible;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
