using Microsoft.Xaml.Interactivity;
using NeilX.DoubanFM.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace NeilX.DoubanFM.Behaviors
{
    public class PlayAnimationBehavior:Behavior<FrameworkElement>
    {
        private static Grid _shell;
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
                if (_shell == null) return;
            } 
            Point ap= AssociatedObject.TransformToVisual(_shell).TransformPoint(new Point());
            Point tp = target.TransformToVisual(_shell).TransformPoint(new Point());
            Debug.WriteLine("a.X"+ap.X);
            Debug.WriteLine("a.Y" + ap.Y);
            Debug.WriteLine("t.X" + tp.X);
            Debug.WriteLine("t.Y" + tp.Y);
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


        Grid GetParentContainer()
        {
            DependencyObject d = VisualTreeHelper.GetParent(AssociatedObject);
            while ((d as Grid)?.Name != "RootContainer" && d!=null)
            {
                d = VisualTreeHelper.GetParent(d);
            }
            return (Grid)d;
        }
    }
}
