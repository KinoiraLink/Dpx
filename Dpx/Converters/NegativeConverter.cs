using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Dpx.Utils;
using Xamarin.Forms;

namespace Dpx.Converters
{
    public class NegativeConverter : IValueConverter
    {
        /// <summary>
        /// 取反转换器
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b && !b;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallMeException();
        }
    }
}
