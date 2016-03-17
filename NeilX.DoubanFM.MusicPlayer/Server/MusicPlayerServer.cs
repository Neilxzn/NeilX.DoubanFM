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

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    public sealed class MusicPlayerServer:IMediaPlayer    
    {
        public  const string BackgroundMediaPlayerActivatedMessageKey = @"Activated";
        public const string BackgroundMediaPlayerUserMessageKey = @"UserMessage";
        private IMusicPlayerServerHandler _audioHandler;
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
                return  mediaPlayer?.SystemMediaTransportControls;
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

        public void InitialServer()
        {

            ConfigureMediaPlayer();

            ActivateHandler();

            AttachMessageHandlers();


          
        }




        void SendMessageToClient(string tag,string message)
        {
            ValueSet valueset = new ValueSet();
            valueset.Add("MessageId", BackgroundMediaPlayerUserMessageKey);
            valueset.Add("MessageTag", tag);
            valueset.Add("MessageContent", message);
            BackgroundMediaPlayer.SendMessageToForeground(valueset);
        }






        private void ActivateHandler()
        {
            _audioHandler = new MusicPlayerServerHandler();
            _audioHandler.OnActivated(this);
        }

        private void AttachMessageHandlers()
        {
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;
        }




        private void ConfigureMediaPlayer()
        {
            mediaPlayer = BackgroundMediaPlayer.Current;
           // mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            mediaPlayer.AudioDeviceType = MediaPlayerAudioDeviceType.Multimedia;
            mediaPlayer.AutoPlay = false;

            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.SeekCompleted += MediaPlayer_SeekCompleted;
            mediaPlayer.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
        }

       

        private void Shutdown()
        {
            mediaPlayer.MediaOpened -= MediaPlayer_MediaOpened;
            mediaPlayer.MediaFailed -= MediaPlayer_MediaFailed;
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            mediaPlayer.SeekCompleted -= MediaPlayer_SeekCompleted;
            mediaPlayer.CurrentStateChanged -= MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;

        }

        private void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            var valueset = e.Data;
            if (valueset["MessageId"] as string==BackgroundMediaPlayerUserMessageKey)
            {
                _audioHandler?.OnReceiveMessage(valueset["MessageTag"] as string, valueset["MessageContent"] as string);
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

        public void SetMediaSource(Song song )
        {
            //IMediaSource source = MediaSource.CreateFromUri(new Uri(mediaSource.Url));
            //MediaStreamSource streamSource;
            //mediaPlayer?.SetMediaSource(source);
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
            mediaPlayer?.Pause();
        }
    }
}
