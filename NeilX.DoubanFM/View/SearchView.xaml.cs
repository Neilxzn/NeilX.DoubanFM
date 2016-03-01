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

namespace NeilX.DoubanFM.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchView : Page
    {
        public SearchViewModel SearchVM => (SearchViewModel)DataContext;
        public SearchView()
        {
            this.InitializeComponent();
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var channel = e.ClickedItem as Channel;
            DoubanFMService.ChangeFMChannel(channel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
