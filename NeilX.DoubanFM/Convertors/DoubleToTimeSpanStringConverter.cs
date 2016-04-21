using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace NeilX.DoubanFM.Convertors
{

    public class IntToTimeSpanStringConverter : IValueConverter
    {
        private string format = @"hh\:mm\:ss";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int dv = (int)value;
            TimeSpan newvalue= TimeSpan.FromSeconds(dv);
                string result = newvalue.ToString(format);
            if (newvalue.Hours > 0)
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
            throw new NotImplementedException();
        }
    }
}
