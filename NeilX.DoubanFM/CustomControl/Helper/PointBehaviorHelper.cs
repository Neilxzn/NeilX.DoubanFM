using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace NeilX.DoubanFM.Helper.CustomControl
{
    public static class PointBehaviorHelper
    {




        public static Brush GetPointEnterBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PointEnterBackgroundProperty);
        }

        public static void SetPointEnterBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(PointEnterBackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for PointEnterBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointEnterBackgroundProperty =
            DependencyProperty.RegisterAttached("PointEnterBackground", typeof(Brush), typeof(PointBehaviorHelper), new PropertyMetadata(null));
       




    }
}
