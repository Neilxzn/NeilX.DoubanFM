using GalaSoft.MvvmLight.Messaging;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.View.Flyout;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NeilX.DoubanFM.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyMusicView : Page
    {
        public static readonly Guid Token = Guid.NewGuid();
        public MyMusicViewModel MyMusicVM => (MyMusicViewModel)DataContext;

        public MyMusicView()
        {
            this.InitializeComponent();
            InitializeMessenger();
        }

        public void InitializeMessenger()
        {
        }





        private void SongGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void songListsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongList list = e.ClickedItem as SongList;
            Messenger.Default.Send(new NotificationMessage<SongList>(list, "GotoSongListView"), Token);

        }

        private void addSongListBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.NavigationService.ShowCenterFlyout(new AddSongListFlyout());
        }



        private void playListButton_Click(object sender, RoutedEventArgs e)
        {
            var allSongs = ViewModelLocator.Instance.MyMusicVM.AllMusicSongs;
            SongList list = ((Button)sender).DataContext as SongList;
            List<Song> songs = (from s in allSongs
                         where s.Song.ListId == list.Id
                         select s.Song).ToList();
            if (songs == null&&songs.Count<1) return;
            ViewModelLocator.Instance.Main.PlayerSession.SetPlaylist(songs, songs[0]);
        }
    }
}
