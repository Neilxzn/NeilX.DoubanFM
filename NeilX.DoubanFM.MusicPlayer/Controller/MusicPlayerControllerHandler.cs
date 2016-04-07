using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeilX.DoubanFM.Core;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerControllerHandler : IMusicPlayerControllerHandler
    {
        public void NotifyControllerReady()
        {
            
        }




        public void NotifyControllerStateChanged(MediaPlayerState state)
        {
            MessageService.SendMessageToClient(new CurrentStateChangedMessage(state));

        }




        public void NotifyCurrentTrackChanged(Song track)
        {
            MessageService.SendMessageToClient(new TrackChangedMessage(track));

        }



        public void NotifyMediaEnd()
        {
            MessageService.SendMessageToClient(new PlayerEventMessage(PlayerEventMessage.MediaEnd, ""));

        }

        public void NotifyMediaFailed()
        {
            MessageService.SendMessageToClient(new PlayerEventMessage(PlayerEventMessage.MediaFailed, ""));

        }

        public void NotifyMediaOpened()
        {
            MessageService.SendMessageToClient(new PlayerEventMessage(PlayerEventMessage.MediaOpened, ""));

        }

        public void NotifySeekCompleted()
        {
            MessageService.SendMessageToClient(new PlayerEventMessage(PlayerEventMessage.SeekCompleted, ""));
        }
        public void NotifyPlaylist(IList<Song> playlist)
        {
            throw new NotImplementedException();
        }



        public void NotifyPosition(TimeSpan position)
        {
            throw new NotImplementedException();
        }
        public void NotifyDuration(TimeSpan? duration)
        {
            throw new NotImplementedException();
        }
    }
}
