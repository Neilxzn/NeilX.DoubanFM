using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.Selector
{
    public class FMsItemTemplateSelector:DataTemplateSelector
    {
        public DataTemplate ArtistFMTempalte { get; set; }
        public DataTemplate SingleSongFMTempalte { get; set; }
        public DataTemplate YearsFMTempalte { get; set; }
        public DataTemplate StyleSongFMTempalte { get; set; }
        public DataTemplate MoodFMTempalte { get; set; }
        public DataTemplate BrandFMTempalte { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            Channel channel = item as Channel;
            return ArtistFMTempalte;
            return base.SelectTemplateCore(item, container);
        }

    }
}
