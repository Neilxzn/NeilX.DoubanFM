using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using NeilX.DoubanFM.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilSoft.UWP;

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    public sealed class MusicPlayerServer : IMediaPlayer
    {

        private IMusicPlayerServerHandler _musicPlayerHandler;
        private MediaPlayer mediaPlayer;

        public event TypedEventHandler<IMediaPlayer, object> MediaOpened;
        public event TypedEventHandler<IMediaPlayer, object> MediaEnded;
        public event TypedEventHandler<IMediaPlayer, MediaPlayerFailedEventArgs> MediaFailed;
        public event TypedEventHandler<IMediaPlayer, object> CurrentStateChanged;
        public event TypedEventHandler<IMediaPlayer, object> SeekCompleted;

        public SystemMediaTransportControls SystemMediaTransportControls
        {
            get
            {
                return mediaPlayer?.SystemMediaTransportControls;
            }
        }

        public bool IsMuted
        {
            get
            {
                return mediaPlayer.IsMuted;
            }

            set
            {
                mediaPlayer.IsMuted = value;
            }
        }

        public TimeSpan Position
        {
            get
            {
                return mediaPlayer.Position;
            }

            set
            {
                mediaPlayer.Position = value;
            }
        }

        public double Volume
        {
            get
            {
                return mediaPlayer.Volume;
            }

            set
            {
                mediaPlayer.Volume = value;
            }
        }

        public MediaPlayerState CurrentState
        {
            get
            {
                return mediaPlayer.CurrentState;
            }
        }

        public void InitialServer()
        {

            ConfigureMediaPlayer();

            ActivateHandler();

            AttachMessageHandlers();



        }


        private void ActivateHandler()
        {
            _musicPlayerHandler = new MusicPlayerServerHandler();
            _musicPlayerHandler.OnActivated(this);
        }

        private void AttachMessageHandlers()
        {
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;
        }




        private void ConfigureMediaPlayer()
        {
            mediaPlayer = BackgroundMediaPlayer.Current;
            mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            mediaPlayer.AudioDeviceType = MediaPlayerAudioDeviceType.Multimedia;
            mediaPlayer.AutoPlay = false;

            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.SeekCompleted += MediaPlayer_SeekCompleted;
            mediaPlayer.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
        }



        public void Shutdown()
        {
            _musicPlayerHandler?.OnCanceled();
            mediaPlayer.MediaOpened -= MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed -= MediaPlayer_MediaFailed;
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            mediaPlayer.SeekCompleted -= MediaPlayer_SeekCompleted;
            mediaPlayer.CurrentStateChanged -= MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;
           // BackgroundMediaPlayer.Shutdown();

        }

        private void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            var valueset = e.Data;
            if (valueset[MessageService.MessageId] as string == MessageService.BackgroundMediaPlayerUserMessageKey)
            {
                MessageType type = EnumHelper.Parse<MessageType>(valueset[MessageService.MessageType] as string);
                _musicPlayerHandler?.OnReceiveMessage(type, valueset[MessageService.MessageContent] as string);
            }
        }
        private void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            
            CurrentStateChanged(this, args);
        }

        private void MediaPlayer_SeekCompleted(MediaPlayer sender, object args)
        {
            SeekCompleted(this, args);
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            MediaEnded(this, args);
        }

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            MediaFailed(this, args);
        }

        private void MediaPlayer_MediaOpened(MediaPlayer sender, object args)
        {
            MediaOpened(this, args);
        }

        public  async void SetMediaSource(Song song)
        {
            Uri uri;
            if (Uri.TryCreate(song.Url,UriKind.RelativeOrAbsolute,out uri))
            {
                if (uri.IsFile)
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(uri.LocalPath);
                    mediaPlayer.SetFileSource(file);     
                }
                else
                {
                    mediaPlayer.SetUriSource(uri);
                }
            }
            else
            {
                throw new NotSupportedException("Not supported uri.");
            }

        }

        public void SetMediaStreamSource(MediaStreamSource streamSource)
        {
            mediaPlayer?.SetMediaSource(streamSource);
        }

        public void Play()
        {
            mediaPlayer?.Play();
        }

        public void Pause()
        {
            if (mediaPlayer.CanPause)
                mediaPlayer?.Pause();
        }

        private async void SetMediaSourceByUri(Uri uri)
        {
            StorageFile file;
            if (uri.Scheme == "ms-appx")
                file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            else if (uri.IsFile)
                file = await StorageFile.GetFileFromPathAsync(uri.LocalPath);
            else
                throw new NotSupportedException("Not supported uri.");
           
        }
    }
}
