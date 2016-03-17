using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer.Server;
using Windows.Media;
using System.Diagnostics;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerController:IMusicPlayerController
    {
        private readonly IMediaPlayer _player;
        private  IMusicPlayerControllerHandler _controllerHandler;
        private IList<Song> _playlist;
        private readonly MediaTransportControls _mtControls;
        private Song _currentTrack;
        private bool _playbackStartedPreviously ;

        public MusicPlayerController(IMediaPlayer musicPlayer)
        {
            _player = musicPlayer;
            _controllerHandler = new MusicPlayerControllerHandler();
            _playbackStartedPreviously = false;

            #region musicplayer
            _player.SeekCompleted += _musicPlayer_SeekCompleted;
            _player.MediaOpened += _musicPlayer_MediaOpened;
            _player.MediaEnded += _musicPlayer_MediaEnded;
            _player.MediaFailed += _musicPlayer_MediaFailed;
            _player.CurrentStateChanged += _musicPlayer_CurrentStateChanged;
            #endregion

            _mtControls = new MediaTransportControls(_player.SystemMediaTransportControls);
            _mtControls.IsEnabled = _mtControls.IsPauseEnabled = _mtControls.IsPlayEnabled = true;
            _mtControls.ButtonPressed += _mtControls_ButtonPressed;

        }

        private void _mtControls_ButtonPressed(object sender, SystemMediaTransportControlsButtonPressedEventArgs e)
        {
            switch (e.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Debug.WriteLine("UVC play button pressed");
                    //StartPlayback();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Debug.WriteLine("UVC pause button pressed");
                    try
                    {
                        BackgroundMediaPlayer.Current.Pause();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Debug.WriteLine("UVC next button pressed");
                    //SkipToNext();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Debug.WriteLine("UVC previous button pressed");
                    //SkipToPrevious();
                    break;
            }
        }

        public void OnReceiveMessage(object message)
        {
            
        }

        public void OnCanceled()
        {
            _player.SeekCompleted -= _musicPlayer_SeekCompleted;
            _player.MediaOpened -= _musicPlayer_MediaOpened;
            _player.MediaEnded -= _musicPlayer_MediaEnded;
            _player.MediaFailed -= _musicPlayer_MediaFailed;
            _player.CurrentStateChanged -= _musicPlayer_CurrentStateChanged;
        }
        #region Player Event Handler
        private void _musicPlayer_CurrentStateChanged(IMediaPlayer sender, object args)
        {
            // _controllerHandler?.NotifyControllerStateChanged(sender.CurrentState);
        }

        private void _musicPlayer_MediaFailed(IMediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            _controllerHandler?.NotifyMediaFailed();
        }

        private void _musicPlayer_MediaEnded(IMediaPlayer sender, object args)
        {
            _controllerHandler?.NotifyMediaEnd();
        }

        private void _musicPlayer_MediaOpened(IMediaPlayer sender, object args)
        {
            _controllerHandler?.NotifyMediaOpened();
        }

        private void _musicPlayer_SeekCompleted(IMediaPlayer sender, object args)
        {
            _controllerHandler?.NotifySeekCompleted();
        } 
        #endregion

        #region musicplayer event handler


        //private void _musicPlayer_OnReceiveMessage(Windows.Foundation.Collections.ValueSet message)
        //{
        //    TrackChangedMessage trackChangedMessage;
        //    if (MessageService.TryParseMessage(message, out trackChangedMessage))
        //    {
        //        // When foreground app is active change track based on background message
        //        _controllerHandler?.NotifyCurrentTrackChanged(_playlist?.FirstOrDefault(s => s.Url == trackChangedMessage.SongUri));
        //        _currentTrack = _playlist.FirstOrDefault(s => s.Url == trackChangedMessage.SongUri);
        //    }
        //}

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
            _controllerHandler.NotifyPlaylist(tracks);
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
            _controllerHandler?.NotifyPlaylist(_playlist);
        }

        public void AskCurrentTrack()
        {
            _controllerHandler.NotifyCurrentTrackChanged(_currentTrack);
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
