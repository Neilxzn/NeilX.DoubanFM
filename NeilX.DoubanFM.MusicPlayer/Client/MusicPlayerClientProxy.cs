using NeilX.DoubanFM.MusicPlayer.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using NeilX.DoubanFM.Core;

namespace NeilX.DoubanFM.MusicPlayer.Client
{
    public class MusicPlayerClientProxy : IMusicPlayerController
    {
        private readonly MediaPlayer currentMediaPlayer;

        public void AskCurrentState()
        {
            throw new NotImplementedException();
        }

        public void AskCurrentTrack()
        {
            throw new NotImplementedException();
        }

        public void AskDuration()
        {
            throw new NotImplementedException();
        }

        public void AskPlaylist()
        {
            throw new NotImplementedException();
        }

        public void AskPosition()
        {
            throw new NotImplementedException();
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrevious()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentTrack(Song track)
        {
            throw new NotImplementedException();
        }

        public void SetPlaylist(IList<Song> tracks)
        {
            throw new NotImplementedException();
        }

        public void SetPlayMode(PlayMode playmode)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(TimeSpan position)
        {
            throw new NotImplementedException();
        }

        public void SetupHandler()
        {
            
        }

        public void SetVolume(double value)
        {
            throw new NotImplementedException();
        }
        public MusicPlayerClientProxy(MediaPlayer player)
        {
            currentMediaPlayer = player;
        }
    }
}
