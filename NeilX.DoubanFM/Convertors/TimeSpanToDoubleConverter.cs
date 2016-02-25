using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace NeilX.DoubanFM.Convertors
{
   public class TimeSpanToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan newvalue = (TimeSpan?)value ?? TimeSpan.Zero;
            return newvalue.TotalMilliseconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double newvalue = (double?)value ?? 0.0;
            return TimeSpan.FromMilliseconds(newvalue);
        }
    }
}
