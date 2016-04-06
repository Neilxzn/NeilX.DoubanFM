using NeilX.DoubanFM.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace NeilX.DoubanFM.Convertors
{
    public class PlayModeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PlayMode playmode = (PlayMode)value;
            string result="";
            switch (playmode)
            {
                case PlayMode.RepeatOne:
                    result= "\uE8ED";
                    break;
                case PlayMode.RepeatAll:
                    result= "\uE8EE";
                    break;
                case PlayMode.List:
                    result= "\uEA37";
                        break;
                case PlayMode.Shuffle:
                    result= "\uE8B1";
                    break;
                default:
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
