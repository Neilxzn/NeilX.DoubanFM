using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.Selector
{
    public class ChlGpHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArtistChannelHeader { get; set; }

        public DataTemplate SingleSongChannelHeader { get; set; }
        public DataTemplate YearsChannelHeader { get; set; }
        public DataTemplate StyleSongChannelHeader { get; set; }
        public DataTemplate MoodChannelHeader { get; set; }
        public DataTemplate BrandChannelHeader { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return ArtistChannelHeader;
        }
    }
}
