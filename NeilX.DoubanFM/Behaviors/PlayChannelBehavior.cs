using Microsoft.Xaml.Interactivity;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NeilX.DoubanFM.Behaviors
{
    public class PlayChannelBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {

            AssociatedObject.PointerReleased += AssociatedObject_PointerReleased;
            AssociatedObject.PointerEntered += AssociatedObject_PointerEntered;
            AssociatedObject.PointerExited += AssociatedObject_PointerExited;
        }

        private void AssociatedObject_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            AssociatedObject.Opacity = 1;
        }

        private void AssociatedObject_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            AssociatedObject.Opacity = 0.7;
        }

        protected override void OnDetaching()
        {
     
            AssociatedObject.PointerReleased -= AssociatedObject_PointerReleased;
        }

        private async void AssociatedObject_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Channel channel = AssociatedObject.DataContext as Channel;
            if (channel != null)
            {
                List<Song> songs = await DoubanFMService.GetSongsFromChannel(channel);
                ViewModelLocator.Instance.Main.PlayerSession.SetPlaylist(songs, songs[0]);
            }
        }
    }
}
