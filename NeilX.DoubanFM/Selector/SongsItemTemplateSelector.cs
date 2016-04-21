using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.Selector
{
    public class SongsItemTemplateSelector: DataTemplateSelector
    {
        private static int _index;
        public DataTemplate LightHeaderTemplate { get; set; }
        public DataTemplate DeepHeaderTemplate { get; set; }

       
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (_index++%2==0)
            {
                return DeepHeaderTemplate;
            }
            else
            {
                return LightHeaderTemplate;
            }
        }
    }
}
