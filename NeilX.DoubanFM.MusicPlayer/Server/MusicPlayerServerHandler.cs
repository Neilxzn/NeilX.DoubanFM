using NeilSoft.UWP;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer.Controller;
using NeilX.DoubanFM.MusicPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    public class MusicPlayerServerHandler : IMusicPlayerServerHandler
    {
        private MusicPlayerController _musicPlayerController;


        public void OnActivated(MusicPlayerServer mediaPlayer)
        {
            _musicPlayerController = new MusicPlayerController(mediaPlayer);
        }

        public void OnCanceled()
        {
            _musicPlayerController?.OnCanceled();
        }

        public void OnReceiveMessage(MessageType type, string message)
        {
            switch (type)
            {
                case MessageType.AppResumedMessage:
                    break;
                case MessageType.AppSuspendedMessage:
                    break;
                case MessageType.AudioTaskStartedMessage:
                    break;
                case MessageType.PlayModeChangeMessage:
                    _musicPlayerController?.SetPlayMode(JsonHelper.FromJson<PlayModeChangeMessage>(message).Playmode);
                    break;
                case MessageType.SkipNextMessage:
                    _musicPlayerController?.MoveNext();
                    break;
                case MessageType.SkipPreviousMessage:
                    _musicPlayerController?.MovePrevious();
                    break;
                case MessageType.StartPlaybackMessage:
                    break;
                case MessageType.TrackChangedMessage:
                    _musicPlayerController?.SetCurrentTrack(JsonHelper.FromJson<TrackChangedMessage>(message).Song);
                    break;
                case MessageType.UpdatePlaylistMessage:
                    _musicPlayerController?.SetPlaylist(JsonHelper.FromJson<UpdatePlaylistMessage>(message).Tracks);
                    break;
                default:
                    break;
            }
            //if (type == "1")
            //    _musicPlayerController?.OnReceiveMessage( message);
            //else
            //{
            //    Debug.WriteLine($"Server: Client Message: {tag}, {message}");
            //}
        }
    }
}
