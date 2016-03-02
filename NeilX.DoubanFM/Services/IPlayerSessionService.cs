using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using NeilX.DoubanFM.MusicPlayer;
using NeilX.DoubanFM.Core;

namespace NeilX.DoubanFM.Services
{
    public interface IPlayerSessionService 
    {
        bool CanPrevious { get; }
        bool CanPause { get; }
        bool CanPlay { get; }
        bool IsPlaying { get; }
        bool CanNext { get; }
        Song CurrentTrack { get; }
        MediaPlaybackStatus PlaybackStatus { get; }
        TimeSpan? Duration { get; }
        TimeSpan Position { get; set; }
        bool IsMuted { get; set; }
        double Volume { get; set; }
        PlayMode PlayMode { get; }

        void RequestPlayOrPause();
        void RequestPrevious();
        void RequestNext();
        void SetPlaylist(IList<Song> tracks, Song current);
        void PlayWhenOpened();
        void ScrollPlayMode();
    }
}
