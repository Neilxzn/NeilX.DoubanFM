using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace NeilX.DoubanFM.Convertors
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        private string format = @"hh\:mm\:ss";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            TimeSpan newvalue = (TimeSpan?)value ?? TimeSpan.Zero;
            string result = newvalue.ToString(format);
            if (newvalue.Hours>0)
            {
                return result;
            }
            else
            {
                return result.Substring(3);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //ToDO
            return TimeSpan.Zero;
        }
    }
}
