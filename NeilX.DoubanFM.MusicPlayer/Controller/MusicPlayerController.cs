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

        public MusicPlayerController(IMediaPlayer musicPlayer)
        {
            _player = musicPlayer;
            _controllerHandler = new MusicPlayerControllerHandler();

            #region musicplayer
            _player.SeekCompleted += _player_SeekCompleted;
            _player.MediaOpened += _player_MediaOpened;
            _player.MediaEnded += _player_MediaEnded;
            _player.MediaFailed += _player_MediaFailed;
            _player.CurrentStateChanged += _player_CurrentStateChanged;
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

       

        public void OnCanceled()
        {
            _player.SeekCompleted -= _player_SeekCompleted;
            _player.MediaOpened -= _player_MediaOpened;
            _player.MediaEnded -= _player_MediaEnded;
            _player.MediaFailed -= _player_MediaFailed;
            _player.CurrentStateChanged -= _player_CurrentStateChanged;
        }

        #region Player Event Handler
        private void _player_CurrentStateChanged(IMediaPlayer sender, object args)
        {
             _controllerHandler?.NotifyControllerStateChanged(sender.CurrentState);
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Closed:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                case MediaPlayerState.Opening:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Changing;
                    break;
                case MediaPlayerState.Buffering:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Changing;
                    break;
                case MediaPlayerState.Playing:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaPlayerState.Paused:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaPlayerState.Stopped:
                    _mtControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                default:
                    break;
            }
        }

        private void _player_MediaFailed(IMediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            _controllerHandler?.NotifyMediaFailed();
        }

        private void _player_MediaEnded(IMediaPlayer sender, object args)
        {
            _controllerHandler?.NotifyMediaEnd();
        }

        private void _player_MediaOpened(IMediaPlayer sender, object args)
        {
            _controllerHandler?.NotifyMediaOpened();
        }

        private void _player_SeekCompleted(IMediaPlayer sender, object args)
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

        public void SetupHandler(IMusicPlayerControllerHandler handler)
        {
            //todo
        }


        public void Play()
        {
            _player.Play();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void SetPlaylist(IList<Song> tracks)
        {
            _playlist = tracks;
            _controllerHandler?.NotifyPlaylist(tracks);
        }

        public void SetCurrentTrack(Song song)
        {
            //if (_playlist!=null&& _currentTrack != track&&_playlist.Contains(track))
            //{
            //   // _currentIndex = _playlist.IndexOf(track);
            //    //MessageService.SendMessageToBackground(new TrackChangedMessage(track.Url));
            //    _controllerHandler?.NotifyCurrentTrackChanged(track);

            //}
            _player.SetMediaSource(song);
            _controllerHandler?.NotifyCurrentTrackChanged(song);
        }
        public void MoveNext()
        {
            //TODO
            //MessageService.SendMessageToBackground(new SkipNextMessage());
        }

        public void MovePrevious()
        {
           // MessageService.SendMessageToBackground(new SkipPreviousMessage());
        }

        public void SetPlayMode(PlayMode playmode)
        {
           // MessageService.SendMessageToBackground(new PlayModeChangeMessage(playmode));
        }

        #region  not Use 

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

        public void AskDuration()
        {
            // _playerSession.NotifyDuration(_musicPlayer.CurrentPlayer.NaturalDuration);
        }
        #endregion

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




    }
}
