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
    public sealed partial class ConfirmFlyout : Page
    {
        private Action _confirmAction;
        public ConfirmFlyout()
        {
            this.InitializeComponent();
        }

        public ConfirmFlyout(Action action)
        {
            this.InitializeComponent();
            _confirmAction = action;
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.NavigationService.CloseCenterFlyout();
        }

        private void comfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            _confirmAction?.Invoke();
            ViewModelLocator.Instance.NavigationService.CloseCenterFlyout();
        }
    }
}
