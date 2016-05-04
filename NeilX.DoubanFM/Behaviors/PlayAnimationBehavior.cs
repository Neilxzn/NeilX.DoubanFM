using Microsoft.Xaml.Interactivity;
using NeilX.DoubanFM.Controls;
using NeilX.DoubanFM.View.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace NeilX.DoubanFM.Behaviors
{
    public class PlayAnimationBehavior:Behavior<FrameworkElement>
    {
        private static Grid _shell;
        private static UserControl _animationObject;
        private static Storyboard storyboard;

        protected override void OnAttached()
        {
            AssociatedObject.PointerReleased += AssociatedObject_PointerReleased;
        }


        protected override void OnDetaching()
        {
            AssociatedObject.PointerReleased -= AssociatedObject_PointerReleased;
            _shell = null;
        }

        private void AssociatedObject_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var target = PlayAnimationSetTargetBehavior.PlayAnimationTarget;
            if (target == null) return;
            if (_shell==null)
            {
                _shell = GetParentContainer();
                _animationObject = new AnimationMusic();
                _animationObject.Visibility = Visibility.Collapsed;
                _shell?.Children.Add(_animationObject);
                if (_shell == null) return;
            } 
            Point ap= AssociatedObject.TransformToVisual(_shell).TransformPoint(new Point());
            Point tp = target.TransformToVisual(_shell).TransformPoint(new Point());
            PrepareAnimation(target, tp, AssociatedObject, ap);

           
        }



        private Grid GetParentContainer()
        {
            DependencyObject d = VisualTreeHelper.GetParent(AssociatedObject);
            while ((d as Grid)?.Name != "RootContainer" && d!=null)
            {
                d = VisualTreeHelper.GetParent(d);
            }
            return (Grid)d;
        }

        private void PrepareAnimation(FrameworkElement target,Point tp,FrameworkElement associatedObject, Point ap)
        {
            if (storyboard != null)
            {
                storyboard.Stop();
                storyboard = null;
            }
         
             DoubleAnimation doubleanimationX = new DoubleAnimation();
            doubleanimationX.Duration = TimeSpan.FromSeconds(1.4);
            doubleanimationX.To = tp.X + target.ActualWidth / 2 - _animationObject.ActualWidth / 2;
            doubleanimationX.From = ap.X + AssociatedObject.ActualWidth / 2 - _animationObject.ActualWidth / 2;
            doubleanimationX.AutoReverse = false;
            BounceEase ease = new BounceEase();
            ease.Bounces = 1;
            ease.Bounciness = 40;
            ease.EasingMode = EasingMode.EaseOut;
            doubleanimationX.EasingFunction = ease;
            Storyboard.SetTarget(doubleanimationX, _animationObject);
            Storyboard.SetTargetProperty(doubleanimationX, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            DoubleAnimation doubleanimationY = new DoubleAnimation();
            doubleanimationY.Duration = TimeSpan.FromSeconds(1.4);
            doubleanimationY.To = tp.Y + target.ActualHeight / 2 - _animationObject.ActualHeight / 2;
            doubleanimationY.From = ap.Y + AssociatedObject.ActualHeight / 2 - _animationObject.ActualHeight / 2;
            doubleanimationY.AutoReverse = false;
            Storyboard.SetTarget(doubleanimationY, _animationObject);
            Storyboard.SetTargetProperty(doubleanimationY, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            storyboard = new Storyboard();
            storyboard.Children.Add(doubleanimationX);
            storyboard.Children.Add(doubleanimationY);
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();
            _animationObject.Visibility = Visibility.Visible;
        }
        private void Storyboard_Completed(object sender, object e)
        {
            _animationObject.Visibility = Visibility.Collapsed;
        }
    }
}
