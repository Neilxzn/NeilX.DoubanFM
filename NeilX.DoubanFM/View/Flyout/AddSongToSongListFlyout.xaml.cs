using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NeilX.DoubanFM.View.Flyout
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AddSongToSongListFlyout : Page
    {
        private Song _song;



        public IEnumerable<SongList> SongLists
        {
            get { return (IEnumerable<SongList>)GetValue(SongListsProperty); }
            set { SetValue(SongListsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SongLists.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SongListsProperty =
            DependencyProperty.Register("SongLists", typeof(IEnumerable<SongList>), typeof(AddSongToSongListFlyout), new PropertyMetadata(null));




        public AddSongToSongListFlyout()
        {
            this.InitializeComponent();
          
        }

        public AddSongToSongListFlyout(IEnumerable<SongList> list, Song song)
        {
            this.InitializeComponent();
            SongLists = list;
            _song = song;
            DataContext = this;
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.NavigationService.CloseCenterFlyout();
        }

        private void songListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SongList list = e.ClickedItem as SongList;
            if (list != null)
            {
                LocalDataService.AddSongToSongList(_song, list);
            }
            ViewModelLocator.Instance.MyMusicVM.ReflashData(false);
            ViewModelLocator.Instance.NavigationService.CloseCenterFlyout();
        }
    }
}
