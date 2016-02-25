using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
   public  interface IMusicPlayerControllerHandler
    {

        void NotifyControllerReady();
        void NotifyMediaOpened();
        void NotifyControllerStateChanged(MediaPlayerState state);
        void NotifyCurrentTrackChanged(TrackInfo track);
        void NotifyDuration(TimeSpan? duration);
        void NotifyPosition(TimeSpan position);
        void NotifySeekCompleted();
        void NotifyPlaylist(IList<TrackInfo> playlist);
    }
}
