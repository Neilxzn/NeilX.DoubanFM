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
        public MyMusicViewModel MyMusicVM => (MyMusicViewModel)DataContext;

        public MyMusicView()
        {
            this.InitializeComponent();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

            ContentDialog d = new ContentDialog();
            d.Title = "删除？";
            d.PrimaryButtonText = "确认";
            d.SecondaryButtonText = "取消";
            d.ShowAsync();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            ContentDialog d = new ContentDialog();
            d.Title = "歌曲信息";
            d.IsSecondaryButtonEnabled = false;
            d.PrimaryButtonText = "确认";
            d.Content = "微光 数据格式:MP3，长度：3:30";
            d.ShowAsync();
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            TextBox tb = new TextBox();
            tb.Text = "列表1";
            ContentDialog d = new ContentDialog();
            d.Title = "重命名";
            d.SecondaryButtonText = "取消";
            d.PrimaryButtonText = "确认";
            d.Content = tb;
            d.ShowAsync();
        }
    }
}
