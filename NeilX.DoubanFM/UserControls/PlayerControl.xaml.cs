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



        public Visibility IsOpenPlayingView
        {
            get { return (Visibility)GetValue(IsOpenPlayingViewProperty); }
            set { SetValue(IsOpenPlayingViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpenPlayingView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenPlayingViewProperty =
            DependencyProperty.Register("IsOpenPlayingView", typeof(Visibility), typeof(PlayerControl), new PropertyMetadata(Visibility.Collapsed));




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
        }


        public void ChangeIsOpenPlayingView()
        {
            if (IsOpenPlayingView == Visibility.Collapsed)
                IsOpenPlayingView = Visibility.Visible;
            else
                IsOpenPlayingView = Visibility.Collapsed;
        }


    }
}
