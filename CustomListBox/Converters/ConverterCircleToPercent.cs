using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CustomListBox.Converters
{
    /// <summary>
    /// 将角度转化成百分比
    /// </summary>
    public class ConverterCircleToPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(double.Parse(value.ToString()) * 10 / 36);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NullReferenceException();
        }


    }
}
