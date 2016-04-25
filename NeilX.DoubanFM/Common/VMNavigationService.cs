using GalaSoft.MvvmLight.Views;
using NeilX.DoubanFM.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.Common
{
    public class VMNavigationService : INavigationService
    {
        private SplitShell _shell;
        private Frame _frame;
        public string CurrentPageKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public void ShowRightFlyout(DependencyObject content)
        {
            _shell.RightFlyoutContent = content;
        }

        public void CloseRightFlyout()
        {
            if (_shell.IsRightFlyoutOpen)
            {
                _shell.HideRightFlyout();
            }
        }

        public void ShowCenterFlyout()
        {

        }

        public void CloseCenterFlyout()
        {

        }


        public void Initialize(SplitShell shell, Frame frame)
        {
            _shell = shell;
            _frame = frame;

        }

        public void Configure(string pageKey,Type pageType)
        {

        }

    }
}
