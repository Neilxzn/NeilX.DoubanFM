using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    public interface IMediaPlayer
    {
        SystemMediaTransportControls SystemMediaTransportControls { get; }
        bool IsMuted { get; set; }
        TimeSpan Position { set; get; }
        double Volume { get; set; }

        MediaPlayerState CurrentState { get; }
        void SetMediaSource(Song song);
        void SetMediaStreamSource(MediaStreamSource streamSource);
        void Play();
        void Pause();

        event TypedEventHandler<IMediaPlayer, object> MediaOpened;
        event TypedEventHandler<IMediaPlayer, object> MediaEnded;
        event TypedEventHandler<IMediaPlayer, MediaPlayerFailedEventArgs> MediaFailed;
        event TypedEventHandler<IMediaPlayer, object> CurrentStateChanged;
        event TypedEventHandler<IMediaPlayer, object> SeekCompleted;

    }
}
