using GalaSoft.MvvmLight.Messaging;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            Messenger.Default.Register<NotificationMessage>(this, MyMusicViewModel.Token, OpenAddSongListView);
        }

        private void OpenAddSongListView(NotificationMessage obj)
        {
            if (obj.Notification=="open")
            {
                testList.Visibility = Visibility.Visible;
            }
            else if(obj.Notification == "close")
            {
                testList.Visibility = Visibility.Collapsed;
            }
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
    }
}
