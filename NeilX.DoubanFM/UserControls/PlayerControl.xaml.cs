using GalaSoft.MvvmLight.Messaging;
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
using NeilX.DoubanFM.Services;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NeilX.DoubanFM.UserControls
{
    public sealed partial class PlayerControl : UserControl
    {


        public bool IsOpenPlayingView
        {
            get { return (bool)GetValue(IsOpenPlayingViewProperty); }
            set { SetValue(IsOpenPlayingViewProperty, value); }
        }

        public static readonly DependencyProperty IsOpenPlayingViewProperty =
            DependencyProperty.Register("IsOpenPlayingView", typeof(bool), typeof(PlayerControl), new PropertyMetadata(false));





        public PlayerSessionService PlayerSession
        {
            get { return (PlayerSessionService)GetValue(PlayerSessionProperty); }
            set { SetValue(PlayerSessionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerSessin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerSessionProperty =
            DependencyProperty.Register("PlayerSession", typeof(PlayerSessionService), typeof(PlayerControl), new PropertyMetadata(DependencyProperty.UnsetValue));



        public PlayerControl()
        {
            this.InitializeComponent();
            RegisterMessenger();
        }


        public void ChangeIsOpenPlayingView()
        {
            IsOpenPlayingView = !IsOpenPlayingView;
        }

        #region Messenget Helper Methods
        private void RegisterMessenger()
        {
           // Messenger.Default.Register<NotificationMessage>(this, PlayingControl.Token, HandlePlayingViewMsg);

        }

        private void UnRegisterMessenger()
        {
            Messenger.Default.Unregister(this);
        }

        private void HandlePlayingViewMsg(NotificationMessage msg)
        {
            if (msg.Notification == "HidePlayingView")
            {
                ChangeIsOpenPlayingView();
            }
        }


        #endregion
    }
}
