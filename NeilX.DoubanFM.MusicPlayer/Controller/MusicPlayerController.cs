using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerController:IMusicPlayerController
    {
        private readonly MusicPlayer _musicPlayer;
        private readonly IMusicPlayerControllerHandler _playerSession;
        private IList<TrackInfo> _playlist;
        private TrackInfo _currentTrack;
        private bool _playbackStartedPreviously ;

        public MusicPlayerController(IMusicPlayerControllerHandler playSession)
        {
            _playerSession = playSession;
            _playbackStartedPreviously = false;

        #region musicplayer
        _musicPlayer = MusicPlayer.Instance;
            _musicPlayer.CurrentPlayer.SeekCompleted += CurrentPlayer_SeekCompleted;
            _musicPlayer.CurrentPlayer.MediaFailed += CurrentPlayer_MediaFailed;
            _musicPlayer.CurrentPlayer.CurrentStateChanged += CurrentPlayer_CurrentStateChanged;
            _musicPlayer.CurrentPlayer.MediaOpened += CurrentPlayer_MediaOpened;
            _musicPlayer.OnReceiveMessage += _musicPlayer_OnReceiveMessage; 
            #endregion
        }

        #region musicplayer event handler

        private void CurrentPlayer_SeekCompleted(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            _playerSession?.NotifySeekCompleted();
        }

        private void CurrentPlayer_MediaFailed(Windows.Media.Playback.MediaPlayer sender, Windows.Media.Playback.MediaPlayerFailedEventArgs args)
        {
            
        }

        private void CurrentPlayer_CurrentStateChanged(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            _playerSession?.NotifyControllerStateChanged(sender.CurrentState);
        }
        private void CurrentPlayer_MediaOpened(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            _playerSession?.NotifyMediaOpened();
        }
        private void _musicPlayer_OnReceiveMessage(Windows.Foundation.Collections.ValueSet message)
        {
            TrackChangedMessage trackChangedMessage;
            if (MessageService.TryParseMessage(message, out trackChangedMessage))
            {
                // When foreground app is active change track based on background message
                _playerSession?.NotifyCurrentTrackChanged(_playlist?.FirstOrDefault(s => s.Source == trackChangedMessage.TrackId));
                _currentTrack = _playlist.FirstOrDefault(s => s.Source == trackChangedMessage.TrackId);
            }
        }

        #endregion

        public void SetupHandler()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            if (!_playbackStartedPreviously)
            {
                _playbackStartedPreviously = true;
                MessageService.SendMessageToBackground(new StartPlaybackMessage());
            }
            else
            {
                _musicPlayer.CurrentPlayer.Play();
            }

        }

        public void Pause()
        {
            _musicPlayer.CurrentPlayer.Pause();
        }

        public void SetPlaylist(IList<TrackInfo> tracks)
        {
            _playlist = tracks;
            _playerSession.NotifyPlaylist(tracks);
            MessageService.SendMessageToBackground(new UpdatePlaylistMessage(tracks));
        }

        public void SetCurrentTrack(TrackInfo track)
        {
            if (_playlist!=null&& _currentTrack != track&&_playlist.Contains(track))
            {
               // _currentIndex = _playlist.IndexOf(track);
                MessageService.SendMessageToBackground(new TrackChangedMessage(track.Source));
                //_playerSession.NotifyCurrentTrackChanged(track);

            }
        }
        public void MoveNext()
        {
            MessageService.SendMessageToBackground(new SkipNextMessage());
        }

        public void MovePrevious()
        {
            MessageService.SendMessageToBackground(new SkipPreviousMessage());
        }

        public void SetPlayMode(PlayMode playmode)
        {
            MessageService.SendMessageToBackground(new PlayModeChangeMessage(playmode));
        }

        public void AskPosition()
        {
            _playerSession?.NotifyPosition(_musicPlayer.CurrentPlayer.Position);
        }

        public void SetPosition(TimeSpan position)
        {
            _musicPlayer.CurrentPlayer.Position = position;
        }

        public void SetVolume(double value)
        {
            _musicPlayer.CurrentPlayer.Volume = value;
        }

        public void AskPlaylist()
        {
            _playerSession?.NotifyPlaylist(_playlist);
        }

        public void AskCurrentTrack()
        {
            _playerSession.NotifyCurrentTrackChanged(_currentTrack);
        }

        public void AskCurrentState()
        {
            _playerSession?.NotifyControllerStateChanged(_musicPlayer.CurrentPlayer?.CurrentState ?? MediaPlayerState.Closed);
        }

        public void AskDuration()
        {
            _playerSession.NotifyDuration(_musicPlayer.CurrentPlayer.NaturalDuration);
        }


    }
}
