using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer.Server;
using Windows.Media;
using System.Diagnostics;
using NeilSoft.UWP;
using Windows.Storage;
using System.IO;
using System.Runtime.Serialization.Json;

namespace NeilX.DoubanFM.MusicPlayer.Controller
{
    public class MusicPlayerController:IMusicPlayerController
    {
        private readonly IMediaPlayer _player;
        private  IMusicPlayerControllerHandler _controllerHandler;
        private PlayList _playlist;
        private bool _autoPlay;
        private readonly MediaTransportControls _mtControls;
        private Song _currentTrack;
        private AppState foregroundAppState = AppState.Unknown;

        public MusicPlayerController(IMediaPlayer musicPlayer)
        {
            foregroundAppState = AppState.Active;
            _player = musicPlayer;
            _controllerHandler = new MusicPlayerControllerHandler();

            #region musicplayer
            _player.SeekCompleted += _player_SeekCompleted;
            _player.MediaOpened += _player_MediaOpened;
            _player.MediaEnded += _player_MediaEnded;
            _player.MediaFailed += _player_MediaFailed;
            _player.CurrentStateChanged += _player_CurrentStateChanged;
            #endregion

            #region stmc

            _mtControls = new MediaTransportControls(_player.SystemMediaTransportControls);
            _mtControls.IsEnabled = _mtControls.IsPauseEnabled = _mtControls.IsPlayEnabled = true;
            _mtControls.ButtonPressed += _mtControls_ButtonPressed; 
            #endregion

            _playlist = new PlayList();

        }
      


        public void OnCanceled()
        {
            _player.SeekCompleted -= _player_SeekCompleted;
            _player.MediaOpened -= _player_MediaOpened;
            _player.MediaEnded -= _player_MediaEnded;
            _player.MediaFailed -= _player_MediaFailed;
            _player.CurrentStateChanged -= _player_CurrentStateChanged;
            _mtControls.ButtonPressed -= _mtControls_ButtonPressed;
            _mtControls?.Dispose();
            AppSettingsHelper.SaveSettingToLocalSettings(AppSettingsConstants.BackgroundTaskState, BackgroundTaskState.Canceled.ToString());
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
            MoveNext();
        }

        private void _player_MediaOpened(IMediaPlayer sender, object args)
        {

            if (_autoPlay)
            {
                Play();
            }
            else
            {
                _autoPlay = true;
            }
            _controllerHandler?.NotifyMediaOpened();
        }

        private void _player_SeekCompleted(IMediaPlayer sender, object args)
        {
          _controllerHandler?.NotifySeekCompleted();
        }
        #endregion

        #region STMC Event Handeler

        private void _mtControls_ButtonPressed(object sender, SystemMediaTransportControlsButtonPressedEventArgs e)
        {
            switch (e.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Debug.WriteLine("UVC play button pressed");
                    Play();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Debug.WriteLine("UVC pause button pressed");
                    Pause();
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Debug.WriteLine("UVC next button pressed");
                    MoveNext();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Debug.WriteLine("UVC previous button pressed");
                    MovePrevious();
                    break;
            }
        }

        #endregion

        #region private helper methods
        private void SetMediaSource(Song song)
        {
            if (song == null) return;
            _player.SetMediaSource(song);
            _mtControls.SetCurrentTrack(song);
            _mtControls.DetectSMTCStatus(song, _playlist, _playlist.PlayMode);

            if (foregroundAppState == AppState.Active)
                _controllerHandler?.NotifyCurrentTrackChanged(song);
            else
                ;//ApplicationSettingsHelper.SaveSettingToLocalSettings(TaskConstant.TrackIdKey, currentTrackId == null ? null : currentTrackId.ToString());

        }
        #endregion

        #region public methods

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
            _playlist.TrackLists = tracks;
        }

        public void SetCurrentTrack(Song song)
        {
            if (song != null)
            {
                _playlist?.CheckCurrentTrack(song);
                SetMediaSource(song);
            }
        }
        public void MoveNext()
        {
            SetMediaSource(_playlist?.MoveNext());

        }

        public void MovePrevious()
        {
            SetMediaSource(_playlist?.MovePrevious());
        }

        public void SetPlayMode(PlayMode playmode)
        {
            _playlist.PlayMode = playmode;
            _mtControls.DetectSMTCStatus(_playlist.GetCurrentTrack(), _playlist, playmode);
        }

        public void AskCurrentTrack()
        {
            _controllerHandler?.NotifyCurrentTrackChanged(_currentTrack);
        }

        #region AppState
        public void SetAppStateSuspended()
        {
            foregroundAppState = AppState.Suspended;
            _playlist.Save();
            AppSettingsHelper.SaveSettingToLocalSettings(AppSettingsConstants.Position, BackgroundMediaPlayer.Current.Position.ToString());
            AppSettingsHelper.SaveSettingToLocalSettings(AppSettingsConstants.Volume, BackgroundMediaPlayer.Current.Volume * 100);
        }
        public void SetAppStateActive()
        {
            foregroundAppState = AppState.Active;
        }
        #endregion

        #endregion

        //#region  not Use 
        //public void AskPlaylist()
        //{

        //}
        //public void AskCurrentState()
        //{
        //}
        //public void AskPosition()
        //{
        //}

        //public void SetPosition(TimeSpan position)
        //{
        //}

        //public void SetVolume(double value)
        //{
        //}

        //public void AskDuration()
        //{
        //}
        //#endregion

      

    }
}
