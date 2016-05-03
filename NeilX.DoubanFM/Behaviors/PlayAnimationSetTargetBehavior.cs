using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NeilX.DoubanFM.Behaviors
{
    public class PlayAnimationSetTargetBehavior : Behavior<FrameworkElement>
    {
        public static FrameworkElement PlayAnimationTarget;

        protected override void OnAttached()
        {
            PlayAnimationTarget = AssociatedObject;
        }

        protected override void OnDetaching()
        {
            PlayAnimationTarget = null;
        }

    }
}
