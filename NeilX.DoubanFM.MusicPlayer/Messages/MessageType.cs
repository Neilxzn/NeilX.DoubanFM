using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    public enum MessageType
    {
        AppResumedMessage,
        AppSuspendedMessage,
        AudioTaskStartedMessage,
        PlayModeChangeMessage,
        SkipNextMessage,
        SkipPreviousMessage,
        StartPlaybackMessage,
        TrackChangedMessage,
        UpdatePlaylistMessage,
        PlayerEventMessage,
        CurrentStateChangedMessage

    }
}
