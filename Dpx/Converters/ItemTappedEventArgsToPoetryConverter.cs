using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Dpx.Models;
using Dpx.Utils;
using Xamarin.Forms;

namespace Dpx.Converters
{
    /// <summary>
    /// ItemTappedEventArgs转换到Poetry类
    /// </summary>
    public class ItemTappedEventArgsToPoetryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value as ItemTappedEventArgs)?.Item as Poetry;

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallMeException();
        }
    }
}
