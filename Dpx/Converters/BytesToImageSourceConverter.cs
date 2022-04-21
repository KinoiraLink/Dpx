using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Dpx.Utils;
using Xamarin.Forms;

namespace Dpx.Converters
{
    /// <summary>
    /// 字节数组到图片的转换
    /// </summary>
    public class BytesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(value is byte[] bytes) ? null : ImageSource.FromStream(() => new MemoryStream(bytes));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new DoNotCallMeException();
    }
}
