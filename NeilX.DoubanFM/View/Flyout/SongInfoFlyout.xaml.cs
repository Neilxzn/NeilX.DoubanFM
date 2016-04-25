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
    public sealed partial class SongInfoFlyout : Page
    {







        public MusicSongViewModel MusicSong
        {
            get { return (MusicSongViewModel)GetValue(MusicSongProperty); }
            set { SetValue(MusicSongProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MusicSong.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MusicSongProperty =
            DependencyProperty.Register("MusicSong", typeof(MusicSongViewModel), typeof(SongInfoFlyout), new PropertyMetadata(null));




        public SongInfoFlyout()
        {
            this.InitializeComponent();
        }


        public SongInfoFlyout(MusicSongViewModel musicsong)
        {
            this.InitializeComponent();
            MusicSong = musicsong;
            DataContext = this;
        }


        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.NavigationService.CloseRightFlyout();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
