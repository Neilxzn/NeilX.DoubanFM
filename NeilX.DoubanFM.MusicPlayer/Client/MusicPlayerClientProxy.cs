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
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.MusicPlayer.Server;
using NeilSoft.UWP;

namespace NeilX.DoubanFM.MusicPlayer.Client
{
    public class MusicPlayerClientProxy : IMediaPlayer
    {
        private readonly MediaPlayer currentPlayer;
        private readonly MusicPlayerClient client;

        public event TypedEventHandler<IMediaPlayer, object> MediaOpened;
        public event TypedEventHandler<IMediaPlayer, object> MediaEnded;
        public event TypedEventHandler<IMediaPlayer, MediaPlayerFailedEventArgs> MediaFailed;
        public event TypedEventHandler<IMediaPlayer, object> CurrentStateChanged;
        public event TypedEventHandler<IMediaPlayer, object> SeekCompleted;
        public event TypedEventHandler<IMediaPlayer, Song> CurrentTrackChanged;

        public SystemMediaTransportControls SystemMediaTransportControls
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsMuted
        {
            get
            {
                return currentPlayer.IsMuted;
            }
            set
            {
                currentPlayer.IsMuted = value;
            }
        }

        public TimeSpan Position
        {
            get
            {
                return currentPlayer.Position;
            }
            set
            {
                currentPlayer.Position = value;
            }
        }

        public double Volume
        {
            get
            {
                return currentPlayer.Volume;
            }

            set
            {
                currentPlayer.Volume = value;
            }
        }

        public MediaPlayerState CurrentState
        {
            get
            {
                return currentPlayer.CurrentState;
            }
        }

        

        public MusicPlayerClientProxy(MusicPlayerClient _client)
        {
            currentPlayer = BackgroundMediaPlayer.Current;
            client = _client;
            client.MessageReceived += Client_MessageReceived;
        }



        public void SetMediaSource(Song song)
        {
            if (client != null)
            {
                MessageService.SendMessageToServer(new TrackChangedMessage(song));
            }
        }

        public void SetMediaStreamSource(MediaStreamSource streamSource)
        {
            throw new NotImplementedException();
        }

        public void SetPlaylist(IList<Song> songs)
        {
            if (client != null)
            {
                MessageService.SendMessageToServer(new UpdatePlaylistMessage(songs));
            }
        }

        public void SetPlayMode(PlayMode playmode)
        {
            if (client != null)
            {
                MessageService.SendMessageToServer(new PlayModeChangeMessage(playmode));
            }
        }


        public void Play()
        {
            currentPlayer?.Play();
        }

        public void Pause()
        {
            currentPlayer?.Pause();
        }

        public void MovePrevious()
        {
            if (client != null)
            {
                MessageService.SendMessageToServer(new SkipPreviousMessage());
            }
        }
        public void MoveNext()
        {
            if (client != null)
            {
                MessageService.SendMessageToServer(new SkipNextMessage());
            }
        }
        #region Client Handle Message Methods 

        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Type)
            {
                case MessageType.AppResumedMessage:
                    break;
                case MessageType.AppSuspendedMessage:
                    break;
                case MessageType.AudioTaskStartedMessage:
                    break;
                case MessageType.PlayModeChangeMessage:
                    break;
                case MessageType.SkipNextMessage:
                    break;
                case MessageType.SkipPreviousMessage:
                    break;
                case MessageType.StartPlaybackMessage:
                    break;
                case MessageType.TrackChangedMessage:
                    if (CurrentTrackChanged != null)
                        CurrentTrackChanged(this, JsonHelper.FromJson<TrackChangedMessage>(e.MessegeContent).Song);
                    break;
                case MessageType.UpdatePlaylistMessage:
                    break;
                case MessageType.PlayerEventMessage:
                    HandlePlayerEvent(e.MessegeContent);
                    break;
                case MessageType.CurrentStateChangedMessage:
                    if (CurrentStateChanged != null)
                        CurrentStateChanged(this, e.MessegeContent);
                    break;
                default:
                    break;
            }
        }


        private void HandlePlayerEvent(string message)
        {
            PlayerEventMessage playerEventevent = JsonHelper.FromJson<PlayerEventMessage>(message);
            switch (playerEventevent.EventName)
            {
                case PlayerEventMessage.SeekCompleted:
                    if (SeekCompleted != null)
                    {
                        SeekCompleted(this, null);
                    }
                    break;
                case PlayerEventMessage.MediaEnd:
                    if (MediaEnded != null)
                    {
                        MediaEnded(this, null);
                    }                    
                    break;
                case PlayerEventMessage.MediaFailed:
                    if (MediaFailed != null)
                    {
                        MediaFailed(this, null);
                    }
                    break;
                case PlayerEventMessage.MediaOpened:
                    if (MediaOpened != null)
                    {
                        MediaOpened(this, null);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
