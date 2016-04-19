using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using NeilX.DoubanFM.MusicPlayer;
using NeilX.DoubanFM.MusicPlayer.Controller;
using Windows.Media.Playback;
using GalaSoft.MvvmLight.Threading;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer.Server;
using NeilX.DoubanFM.MusicPlayer.Client;
using NeilSoft.UWP;

namespace NeilX.DoubanFM.Services
{
    public class PlayerSessionService : ObservableObject, IPlayerSessionService
    {
        #region private filed

        private static MusicPlayerClientProxy _proxy;
        private readonly Timer _askPositionTimer;
        private static readonly TimeSpan _askPositionPeriod = TimeSpan.FromSeconds(0.25);
        private readonly MusicPlayerClient _client;
        private bool _canPrevious;
        private bool _canPause;
        private bool _canPlay;
        private bool _canNext;
        private bool _isPlay;
        private MediaPlaybackStatus _playbackStatus = MediaPlaybackStatus.Closed;
        private IList<Song> _playlist = new List<Song>();
        private Song _currentTrack;
        private TimeSpan? _duration;
        private TimeSpan _position;
        private bool _isMuted;
        private double _volume;
        private PlayMode _playMode;

        #endregion

        #region Properties
        public static bool IsActived;
        public bool CanPrevious
        {
            get { return _canPrevious; }
            private set
            {
                Set(ref _canPrevious, value);

            }
        }

        public bool CanPause
        {
            get { return _canPause; }
            private set { Set(ref _canPause, value); }
        }

        public bool CanPlay
        {
            get { return _canPlay; }
            private set { Set(ref _canPlay, value); }
        }

        public bool IsPlaying
        {
            get { return _isPlay; }
            private set { Set(ref _isPlay, value); }
        }

        public bool CanNext
        {
            get { return _canNext; }
            private set
            {
                Set(ref _canNext, value);

            }
        }

        public MediaPlaybackStatus PlaybackStatus
        {
            get { return _playbackStatus; }
            private set
            {
                if (Set(ref _playbackStatus, value))
                {
                    //_mtService.PlaybackStatus = value;
                    DispatcherHelper.CheckBeginInvokeOnUI(OnCurrentTrackChanged);
                }
            }
        }

        public IList<Song> Playlist
        {
            get { return _playlist; }
            private set
            {
                if (Set(ref _playlist, value ?? new List<Song>()))
                    _proxy?.SetPlaylist(_playlist);
            }
        }

        public Song CurrentTrack
        {
            get { return _currentTrack; }
            private set
            {
                if (Set(ref _currentTrack, value))
                {
                    _proxy?.SetMediaSource(value);
                    OnCurrentTrackChanged();
                }
            }
        }

        public TimeSpan? Duration
        {
            get { return _duration; }
            private set { Set(ref _duration, value); }
        }

        public TimeSpan Position
        {
            get { return _position; }
            set
            {
                var oldValue = _position;
                if (Set(ref _position, value))
                    OnPositionChanged(oldValue, value);
            }
        }

        public bool IsMuted
        {
            get { return _isMuted; }
            set { Set(ref _isMuted, value); }
        }

        public double Volume
        {
            get { return _volume; }
            set
            {
                if (Set(ref _volume, value))
                    OnVolumeChanged();
            }
        }

        public PlayMode PlayMode
        {
            get { return _playMode; }
            set
            {
                if (Set(ref _playMode, value))
                    OnPlayModeChanged();
            }
        }

        #endregion
        public PlayerSessionService()
        {
            _client = new MusicPlayerClient();
            _client.PlayerActivated += _client_PlayerActivated;
            _askPositionTimer = new Timer(OnAskPosition, null, Timeout.InfiniteTimeSpan, _askPositionPeriod);
        }

        private void _client_PlayerActivated(object sender, object e)
        {
            _proxy = _client.Proxy;
            _proxy.SeekCompleted += _proxy_SeekCompleted;
            _proxy.MediaOpened += _proxy_MediaOpened;
            _proxy.MediaFailed += _proxy_MediaFailed;
            _proxy.MediaEnded += _proxy_MediaEnded;
            _proxy.CurrentStateChanged += _proxy_CurrentStateChanged;
            _proxy.CurrentTrackChanged += _proxy_CurrentTrackChanged;
            IsActived = true;
            LoadState();
            OnPlayModeChanged();
            OnVolumeChanged();
            //  _proxy?.SetupHandler(this);
        }





        #region Public Methods

        public void PlayWhenOpened()
        {
            //_autoPlay = true;
            //if (CanPlay)
            //    Play();
        }

        public void RequestPlayOrPause()
        {
            if (IsPlaying)
            {
                if (CanPause)
                    Pause();
            }
            else
            {
                // if (CanPlay)
                Play();
            }


        }

        public void RequestNext()
        {
            if (CanNext)
                MoveNext();
        }

        public void RequestPrevious()
        {
            if (CanPrevious)
                MovePrevious();
        }

        public void SetPlaylist(IList<Song> tracks, Song current)
        {
            Playlist = tracks;
            CurrentTrack = current;
        }

        public void AddSongToPlaylist(Song newSong)
        {
            if (Playlist != null)
            {
                Playlist.Add(newSong);
            }
            else
            {
                Playlist = new List<Song>()
                {
                    newSong
                };

            }
            CurrentTrack = newSong;
        }

        public void ScrollPlayMode()
        {
            var playmodes = Enum.GetValues(typeof(PlayMode)).Cast<PlayMode>().ToList();
            var nextIdx = playmodes.IndexOf(PlayMode) + 1;
            if (nextIdx >= playmodes.Count)
            {
                nextIdx = 0;
            }
            PlayMode = playmodes[nextIdx];
        }
        public void Close()
        {
            if (_proxy != null)
            {
                _proxy.SeekCompleted -= _proxy_SeekCompleted;
                _proxy.MediaOpened -= _proxy_MediaOpened;
                _proxy.MediaFailed -= _proxy_MediaFailed;
                _proxy.MediaEnded -= _proxy_MediaEnded;
                _proxy.CurrentStateChanged -= _proxy_CurrentStateChanged;
                _proxy.CurrentTrackChanged -= _proxy_CurrentTrackChanged;
                _proxy.Close();
            }

            if (_askPositionTimer != null)
            {
                _askPositionTimer.Dispose();
            }
        }
        #endregion

        #region Properties Change Handler

        private void OnPositionChanged(TimeSpan oldValue, TimeSpan value)
        {
            if (PlaybackStatus != MediaPlaybackStatus.Changing)
            {
                if (Math.Abs(oldValue.Subtract(value).TotalMilliseconds) > 100)
                {
                    SuspendAskPosition();
                    _proxy.Position = value;
                }
            }
        }
        private void OnPlayModeChanged()
        {
            _proxy?.SetPlayMode(_playMode);
            DetectPlayerControlStatus();
        }

        private void OnVolumeChanged()
        {
            if (_proxy != null)
                _proxy.Volume = Math.Min(Math.Max(0.0, Volume / 100), 1.0);
        }

        private void OnCurrentTrackChanged()
        {
            DetectPlayerControlStatus();
        }

        #endregion

        #region proxy Handler
        private void _proxy_MediaEnded(IMediaPlayer sender, object args)
        {

        }

        private async void _proxy_CurrentStateChanged(IMediaPlayer sender, object args)
        {
            await DispatcherHelper.RunAsync(() =>
            {
                switch (_proxy.CurrentState)
                {
                    case MediaPlayerState.Closed:
                        CanPlay = CanPause = false;
                        IsPlaying = false;
                        PlaybackStatus = MediaPlaybackStatus.Closed;
                        SuspendAskPosition();
                        break;
                    case MediaPlayerState.Opening:
                        CanPlay = CanPause = false;
                        IsPlaying = false;
                        PlaybackStatus = MediaPlaybackStatus.Changing;
                        break;
                    case MediaPlayerState.Buffering:
                        CanPlay = CanPause = false;
                        IsPlaying = false;
                        PlaybackStatus = MediaPlaybackStatus.Changing;
                        break;
                    case MediaPlayerState.Playing:
                        CanPause = true;
                        CanPlay = true;
                        IsPlaying = true;
                        PlaybackStatus = MediaPlaybackStatus.Playing;
                        ResumeAskPosition();
                        break;
                    case MediaPlayerState.Paused:
                        CanPause = false;
                        CanPlay = true;
                        IsPlaying = false;
                        PlaybackStatus = MediaPlaybackStatus.Paused;
                        SuspendAskPosition();
                        break;
                    case MediaPlayerState.Stopped:
                        PlaybackStatus = MediaPlaybackStatus.Stopped;
                        SuspendAskPosition();
                        break;
                    default:
                        break;
                }
            });
        }

        private async void _proxy_CurrentTrackChanged(IMediaPlayer sender, Song arg)
        {
            if (_playlist != null)
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    if (Set(ref _currentTrack, arg))
                    {
                        RaisePropertyChanged(nameof(CurrentTrack));
                        OnCurrentTrackChanged();
                    }
                    _position = TimeSpan.Zero;
                    RaisePropertyChanged(nameof(Position));
                    Duration = TimeSpan.FromSeconds((double)arg?.Length);
                });
            }
        }

        private void _proxy_MediaFailed(IMediaPlayer sender, MediaPlayerFailedEventArgs args)
        {

        }

        private void _proxy_MediaOpened(IMediaPlayer sender, object args)
        {
        }

        private void _proxy_SeekCompleted(IMediaPlayer sender, object args)
        {
            ResumeAskPosition();
        }
        #endregion

        #region private Helper  Methods

        #region Player Control Methods

        private void Play()
        {
            PlaybackStatus = MediaPlaybackStatus.Changing;
            _proxy?.Play();
        }

        private void Pause()
        {
            PlaybackStatus = MediaPlaybackStatus.Changing;
            _proxy?.Pause();
        }

        private void MoveNext()
        {
            PlaybackStatus = MediaPlaybackStatus.Changing;
            _proxy?.MoveNext();
        }

        private void MovePrevious()
        {
            PlaybackStatus = MediaPlaybackStatus.Changing;
            _proxy?.MovePrevious();
        }

        #endregion

        #region Position Methods

        private async void OnAskPosition(object state)
        {
            await DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (PlaybackStatus == MediaPlaybackStatus.Playing)
                {
                    _position = _proxy.Position;
                    RaisePropertyChanged(nameof(Position));

                    if (_position.TotalMilliseconds >= Duration.Value.TotalMilliseconds)
                    {
                        SuspendAskPosition();
                    }
                }
            });
        }

        private void SuspendAskPosition()
        {
            _askPositionTimer.Change(Timeout.InfiniteTimeSpan, _askPositionPeriod);
        }

        private void ResumeAskPosition()
        {
            _askPositionTimer.Change(TimeSpan.Zero, _askPositionPeriod);
        }

        #endregion

        #region Laod and Detect State

        private async void LoadState()
        {
            PlayList list = await PlayList.Load();
            double _volumn = AppSettingsHelper.LoadSetttingFromLocalSettings<double>(AppSettingsConstants.Volume, 50);
            await DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                 () =>
                 {
                     Volume = _volumn;
                     if (list!=null)
                     {
                         PlayMode = list.PlayMode;
                     }
                     else
                     {
                         PlayMode = PlayMode.RepeatAll;
                     }
                     if (list?.TrackLists != null)
                     {
                         SetPlaylist(list.TrackLists, list.TrackLists[list.CurrentIndex]);
                     }
                 });

        }

        private void DetectPlayerControlStatus()
        {
            if (CurrentTrack != null)
            {
                var idx = Playlist.IndexOf(CurrentTrack);
                if (idx != -1)
                {
                    switch (PlayMode)
                    {
                        case PlayMode.RepeatOne:
                            CanPrevious = CanNext = true;
                            break;
                        case PlayMode.RepeatAll:
                            CanPrevious = CanNext = true;
                            break;
                        case PlayMode.List:
                            CanPrevious = idx > 0 && PlaybackStatus != MediaPlaybackStatus.Changing;
                            CanNext = idx < Playlist.Count - 1 && PlaybackStatus != MediaPlaybackStatus.Changing;
                            break;
                        case PlayMode.Shuffle:
                            CanPrevious = CanNext = true;
                            break;
                        default:
                            break;
                    }

                }
                else
                    CanPrevious = CanNext = false;
            }
            else
                CanPlay = CanPause = CanPrevious = CanNext = false;
        } 
        #endregion

        #endregion




   


    }
}
