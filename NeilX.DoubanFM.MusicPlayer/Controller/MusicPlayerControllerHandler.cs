using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeilX.DoubanFM.Core;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerControllerHandler : IMusicPlayerControllerHandler
    {
        public void NotifyControllerReady()
        {
            throw new NotImplementedException();
        }

        public void NotifyControllerStateChanged(MediaPlayerState state)
        {
            throw new NotImplementedException();
        }

        public void NotifyCurrentTrackChanged(Song track)
        {
            throw new NotImplementedException();
        }

        public void NotifyDuration(TimeSpan? duration)
        {
            throw new NotImplementedException();
        }

        public void NotifyMediaEnd()
        {
            throw new NotImplementedException();
        }

        public void NotifyMediaFailed()
        {
            throw new NotImplementedException();
        }

        public void NotifyMediaOpened()
        {
            throw new NotImplementedException();
        }

        public void NotifyPlaylist(IList<Song> playlist)
        {
            throw new NotImplementedException();
        }

        public void NotifyPosition(TimeSpan position)
        {
            throw new NotImplementedException();
        }

        public void NotifySeekCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
