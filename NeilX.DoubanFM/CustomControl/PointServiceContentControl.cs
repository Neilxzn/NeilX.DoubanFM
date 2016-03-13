using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace NeilX.DoubanFM.CustomControl
{
    public class PointServiceContentControl:ContentControl
    {
        public static UIElement  AnimationObject { get; set; }



        private Grid gridContent;

        protected override async void OnPointerReleased(PointerRoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            PointerPoint _point = e.GetCurrentPoint(root);
            Debug.WriteLine(_point.Position);
            if (root != null)
            {
                //DoubleAnimation doubleanimationX = new DoubleAnimation();
                //doubleanimationX.Duration = TimeSpan.FromSeconds(0.8);
                //doubleanimationX.To = 0;
                //doubleanimationX.From = _point.Position.X;
                //doubleanimationX.AutoReverse = false;
                //Storyboard.SetTarget(doubleanimationX, AnimationObject);
                //Storyboard.SetTargetProperty(doubleanimationX, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
                //DoubleAnimation doubleanimationY = new DoubleAnimation();
                //doubleanimationY.Duration = TimeSpan.FromSeconds(0.8);
                //doubleanimationY.To = 500;
                //doubleanimationY.From = _point.Position.Y;
                //doubleanimationY.AutoReverse = false;
                //Storyboard.SetTarget(doubleanimationY, AnimationObject);
                //Storyboard.SetTargetProperty(doubleanimationY, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
                //Storyboard storyboard = new Storyboard();
                //storyboard.Children.Add(doubleanimationX);
                //storyboard.Children.Add(doubleanimationY);
                ////storyboard.Completed += Storyboard_Completed;
                //AnimationObject.Visibility = Visibility.Visible;
                //storyboard.Begin();


            }
            Channel channel = DataContext as Channel;
            if (channel != null)
            {
                List<Song> songs = await DoubanFMService.GetSongsFromChannel(channel);
                ViewModelLocator.Instance.Main.PlayerSession.SetPlaylist(songs, songs[0]);
            }
            base.OnPointerReleased(e);
            e.Handled = true;
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            this.Opacity = 0.7;
            base.OnPointerEntered(e);
            e.Handled = true;
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            this.Opacity = 1;
            base.OnPointerExited(e);
            e.Handled = true;
        }



    }
}
