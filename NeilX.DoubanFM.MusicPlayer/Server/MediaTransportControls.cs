using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage.Streams;

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    public class MediaTransportControls
    {
        private readonly SystemMediaTransportControls _smtc;

        public bool IsEnabled
        {
            get { return _smtc.IsEnabled; }
            set { _smtc.IsEnabled = value; }
        }

        public bool IsPlayEnabled
        {
            get { return _smtc.IsPlayEnabled; }
            set { _smtc.IsPlayEnabled = value; }
        }

        public bool IsPauseEnabled
        {
            get { return _smtc.IsPauseEnabled; }
            set { _smtc.IsPauseEnabled = value; }
        }

        public MediaPlaybackStatus PlaybackStatus
        {
            get { return _smtc.PlaybackStatus; }
            set { _smtc.PlaybackStatus = value; }
        }

        public bool CanNext
        {
            get { return _smtc.IsNextEnabled; }
            set { _smtc.IsNextEnabled = value; }
        }

        public bool CanPrevious
        {
            get { return _smtc.IsPreviousEnabled; }
            set { _smtc.IsPreviousEnabled = value; }
        }

        public event EventHandler<SystemMediaTransportControlsButtonPressedEventArgs> ButtonPressed;

        public MediaTransportControls(SystemMediaTransportControls smtc)
        {
            _smtc = smtc;
            PlaybackStatus = MediaPlaybackStatus.Closed;
            _smtc.DisplayUpdater.Type = MediaPlaybackType.Music;
            _smtc.DisplayUpdater.Update();
            _smtc.ButtonPressed += _smtc_ButtonPressed;
        }

        public MediaTransportControls()
            :this(SystemMediaTransportControls.GetForCurrentView())
        {

        }

        private void _smtc_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            ButtonPressed?.Invoke(this, args);
        }

        public void SetCurrentTrack(Song value)
        {
            var updater = _smtc.DisplayUpdater;
            var musicProp = updater.MusicProperties;
            if (value != null)
            {
                musicProp.Title = value.Title;
                musicProp.Artist = value.Artist;
                musicProp.AlbumTitle = value.AlbumTitle;
                musicProp.AlbumArtist = value.Artist;
                if (value.PictureUrl != null)
                    updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(value.PictureUrl));
                else
                    updater.Thumbnail = null;
                PlaybackStatus = MediaPlaybackStatus.Playing;
            }
            else
            {
                updater.ClearAll();
                PlaybackStatus = MediaPlaybackStatus.Closed;
                musicProp.Title = string.Empty;
            }
            updater.Update();

            //smtc.PlaybackStatus = MediaPlaybackStatus.Playing;
            //smtc.DisplayUpdater.Type = MediaPlaybackType.Music;
            //smtc.DisplayUpdater.MusicProperties.Title = item.Source.CustomProperties[TaskConstant.TitleKey] as string;

            //var albumArtUri = item.Source.CustomProperties[TaskConstant.AlbumArtKey] as Uri;
            //if (albumArtUri != null)
            //    smtc.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(albumArtUri);
            //else
            //    smtc.DisplayUpdater.Thumbnail = null;
        }

        public void DetectSMTCStatus(Song song,PlayList playlist,PlayMode playMode)
        {
            if (song != null)
            {
                IsPlayEnabled = IsPauseEnabled = true;
                var idx = playlist.TrackLists.IndexOf(song);
                if (idx != -1)
                {
                    switch (playMode)
                    {
                        case PlayMode.RepeatOne:
                            CanPrevious = CanNext = true;
                            break;
                        case PlayMode.RepeatAll:
                            CanPrevious = CanNext = true;
                            break;
                        case PlayMode.List:
                            CanPrevious = idx > 0 && PlaybackStatus != MediaPlaybackStatus.Changing;
                            CanNext = idx < playlist.TrackLists.Count - 1 && PlaybackStatus != MediaPlaybackStatus.Changing;
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
                IsPlayEnabled = IsPauseEnabled = CanPrevious = CanNext = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PlaybackStatus = MediaPlaybackStatus.Closed;
                    _smtc.ButtonPressed -= _smtc_ButtonPressed;
                }
                disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
