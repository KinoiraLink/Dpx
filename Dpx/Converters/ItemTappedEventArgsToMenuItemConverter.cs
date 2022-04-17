using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Dpx.Utils;
using Xamarin.Forms;
using MenuItem = Dpx.Models.MenuItem;

namespace Dpx.Converters
{
    class ItemTappedEventArgsToMenuItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value as ItemTappedEventArgs)?.Item as Models.MenuItem;

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallMeException();
        }
    }
}
