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
    /// 布局到文本对齐转换器
    /// </summary>
    public class LayoutToTextAlignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value as string)
            {
                case Poetry.CenterLayout: 
                    return TextAlignment.Center;
                case Poetry.IndentLayout: 
                    return TextAlignment.Start;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallMeException();
        }
    }
}
