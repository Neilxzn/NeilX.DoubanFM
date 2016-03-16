using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer.Rpc;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerController:IMusicPlayerController
    {
        private readonly IMediaPlayer _musicPlayer;
        private  IMusicPlayerControllerHandler _playerSession;
        private IList<Song> _playlist;
        private Song _currentTrack;
        private bool _playbackStartedPreviously ;
        private static MusicPlayerController _activedinstance;
        private static int i = 0;
        public MusicPlayerController(IMediaPlayer musicPlayer)
        {
            _musicPlayer = musicPlayer;
            // _playerSession = playSession;
            _playbackStartedPreviously = false;

            #region musicplayer
            //_musicPlayer = MusicPlayer.Instance;
            _musicPlayer.SeekCompleted += _musicPlayer_SeekCompleted;
            _musicPlayer.MediaOpened += _musicPlayer_MediaOpened;
            _musicPlayer.MediaEnded += _musicPlayer_MediaEnded;
            _musicPlayer.MediaFailed += _musicPlayer_MediaFailed;
            _musicPlayer.CurrentStateChanged += _musicPlayer_CurrentStateChanged;
            #endregion
            _activedinstance = this;
            i = 1;
        }

        public static IMusicPlayerController GetActivedControllerInstance( IMusicPlayerControllerHandler handler )
        {
            var o = i;
            if (_activedinstance==null)
            {
                throw new Exception("_activedinstance is null");
            }
            _activedinstance._playerSession = handler;
            return _activedinstance;
        }

        public void OnReceiveMessage(object message)
        {
            
        }

        public void OnCanceled()
        {
            _musicPlayer.SeekCompleted -= _musicPlayer_SeekCompleted;
            _musicPlayer.MediaOpened -= _musicPlayer_MediaOpened;
            _musicPlayer.MediaEnded -= _musicPlayer_MediaEnded;
            _musicPlayer.MediaFailed -= _musicPlayer_MediaFailed;
            _musicPlayer.CurrentStateChanged -= _musicPlayer_CurrentStateChanged;
        }
        private void _musicPlayer_CurrentStateChanged(IMediaPlayer sender, object args)
        {
            throw new NotImplementedException();
        }

        private void _musicPlayer_MediaFailed(IMediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void _musicPlayer_MediaEnded(IMediaPlayer sender, object args)
        {
            throw new NotImplementedException();
        }

        private void _musicPlayer_MediaOpened(IMediaPlayer sender, object args)
        {
            throw new NotImplementedException();
        }

        private void _musicPlayer_SeekCompleted(IMediaPlayer sender, object args)
        {
            throw new NotImplementedException();
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
                _playerSession?.NotifyCurrentTrackChanged(_playlist?.FirstOrDefault(s => s.Url == trackChangedMessage.SongUri));
                _currentTrack = _playlist.FirstOrDefault(s => s.Url == trackChangedMessage.SongUri);
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
               // _musicPlayer.CurrentPlayer.Play();
            }

        }

        public void Pause()
        {
            //_musicPlayer.CurrentPlayer.Pause();
        }

        public void SetPlaylist(IList<Song> tracks)
        {
            _playlist = tracks;
            _playerSession.NotifyPlaylist(tracks);
            MessageService.SendMessageToBackground(new UpdatePlaylistMessage(tracks));
        }

        public void SetCurrentTrack(Song track)
        {
            if (_playlist!=null&& _currentTrack != track&&_playlist.Contains(track))
            {
               // _currentIndex = _playlist.IndexOf(track);
                MessageService.SendMessageToBackground(new TrackChangedMessage(track.Url));
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
           // _playerSession?.NotifyPosition(_musicPlayer.CurrentPlayer.Position);
        }

        public void SetPosition(TimeSpan position)
        {
           // _musicPlayer.CurrentPlayer.Position = position;
        }

        public void SetVolume(double value)
        {
            //_musicPlayer.CurrentPlayer.Volume = value;
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
           // _playerSession?.NotifyControllerStateChanged(_musicPlayer.CurrentPlayer?.CurrentState ?? MediaPlayerState.Closed);
        }

        public void AskDuration()
        {
           // _playerSession.NotifyDuration(_musicPlayer.CurrentPlayer.NaturalDuration);
        }


    }
}
