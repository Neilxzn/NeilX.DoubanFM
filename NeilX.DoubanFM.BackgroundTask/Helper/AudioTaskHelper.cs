using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.BackgroundTask.Helper
{
    public sealed class AudioTaskHelper
    {
        public static Uri GetPlayListCurrentTrackId(MediaPlaybackList playbackList)
        {
            if (playbackList == null)
                return null;

            return GetPlaybackListItemTrackId(playbackList.CurrentItem);
        }

        public static  Uri GetPlaybackListItemTrackId(MediaPlaybackItem item)
        {
            if (item == null)
                return null; // no track playing

            return item.Source.CustomProperties[TaskConstant.TrackIdKey] as Uri;
        }


    }
}
