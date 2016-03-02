using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public interface IMusicPlayerController
    {
        void SetupHandler();
        void Play();
        void Pause();
        void SetPlaylist(IList<Song> tracks);
        void SetCurrentTrack(Song track);
        void MoveNext();
        void MovePrevious();
        void SetPlayMode(PlayMode playmode);
        void AskPosition();
        void SetPosition(TimeSpan position);
        void SetVolume(double value);
        void AskPlaylist();
        void AskCurrentTrack();
        void AskCurrentState();
        void AskDuration();

    }
}
