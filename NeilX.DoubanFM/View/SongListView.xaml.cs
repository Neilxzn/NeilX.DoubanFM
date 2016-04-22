using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NeilX.DoubanFM.Core;
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

namespace NeilX.DoubanFM.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SongListView : Page
    {
        public static readonly Guid Token = Guid.NewGuid();
        public SongListViewModel SongListVM => (SongListViewModel)DataContext;
        public SongListView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SongListVM.OnNavigatedTo(e.Parameter as SongList);
        }

        private void EditListBtn_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new NotificationMessage<SongList>(SongListVM.SelectList,"GotoEditView"), Token);
        }

        private void DelectListBtn_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new NotificationMessage<SongList>(null,"Update"), Token);
            SongListVM.DelectSongLits();
            ViewModelLocator.Instance.MyMusicVM.ReflashData();
        }
    }
}
