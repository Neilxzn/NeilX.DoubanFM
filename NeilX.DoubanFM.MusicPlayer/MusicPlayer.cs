using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;

namespace NeilX.DoubanFM.MusicPlayer
{
    public delegate void OnReceiveMessageEventHandler(ValueSet message);
    class MusicPlayer : IDisposable
    {
        #region  private  filed

        const int RPC_S_SERVER_UNAVAILABLE = -2147023174; // 0x800706BA
        private bool _isAudioPlayerTaskRunning = false;
        private AutoResetEvent backgroundAudioTaskStarted;
        private static MusicPlayer instance;

        #endregion

        #region private contructor

        private MusicPlayer()
        {
            backgroundAudioTaskStarted = new AutoResetEvent(false);
            InitializePlayer();
        }

        #endregion

        #region Public Properties

        public event OnReceiveMessageEventHandler OnReceiveMessage;
        public bool IsAudioPlayerTaskRunning
        {
            get
            {
                if (_isAudioPlayerTaskRunning)
                    return true;

                string value = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.BackgroundTaskState) as string;
                if (value == null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        _isAudioPlayerTaskRunning = EnumHelper.Parse<BackgroundTaskState>(value) == BackgroundTaskState.Running;
                    }
                    catch (ArgumentException)
                    {
                        _isAudioPlayerTaskRunning = false;
                    }
                    return _isAudioPlayerTaskRunning;
                }
            }
        }
        public MediaPlayer CurrentPlayer
        {
            get
            {
                MediaPlayer mp = null;
                int retryCount = 2;

                while (mp == null && --retryCount >= 0)
                {
                    mp = BackgroundMediaPlayer.Current;
                }

                if (mp == null)
                {
                    throw new Exception("Failed to get a MediaPlayer instance.");
                }

                return mp;
            }
        }
        public static MusicPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MusicPlayer();

                }
                return instance;
            }

        }

        #endregion

        #region  MediaPlayer Life Pivate Methods
        private void InitializePlayer()
        {
            //if (IsAudioPlayerTaskRunning)
            //{
            //    if (MediaPlayerState.Closed == CurrentPlayer.CurrentState)
            //    {
            //        ResetAfterLostBackground();
            //        StartBackgroundAudioTaskc();
            //    }
            //}
            //else
            //{
                StartBackgroundAudioTaskc();
            //}
        }
        private void StartBackgroundAudioTaskc()
        {

            AddMediaPlayerEventHandlers();
            bool result = backgroundAudioTaskStarted.WaitOne(8000);
            if (result == true)
            {
                _isAudioPlayerTaskRunning = true;
                //   MessageService.SendMessageToBackground(new UpdatePlaylistMessage(PlayList.TrackLists.ToList()));
            }
            else
            {
                throw new Exception("Background Audio Task didn't start in expected time");
            }


        }
        private void ResetAfterLostBackground()
        {
            BackgroundMediaPlayer.Shutdown();
            _isAudioPlayerTaskRunning = false;
            backgroundAudioTaskStarted.Reset();
            ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.BackgroundTaskState, BackgroundTaskState.Unknown.ToString());

            try
            {
                BackgroundMediaPlayer.MessageReceivedFromBackground += BackgroundMediaPlayer_MessageReceivedFromBackground;
            }
            catch (Exception ex)
            {
                if (ex.HResult == RPC_S_SERVER_UNAVAILABLE)
                {
                    throw new Exception("Failed to get a MediaPlayer instance.");
                }
                else
                {
                    throw;
                }
            }
        }

        private void RemoveMediaPlayerEventHandlers()
        {
            try
            {
                BackgroundMediaPlayer.MessageReceivedFromBackground -= BackgroundMediaPlayer_MessageReceivedFromBackground;
            }
            catch (Exception ex)
            {
                if (ex.HResult == RPC_S_SERVER_UNAVAILABLE)
                {
                    // do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        private void AddMediaPlayerEventHandlers()
        {
            try
            {
                BackgroundMediaPlayer.MessageReceivedFromBackground += BackgroundMediaPlayer_MessageReceivedFromBackground;
            }
            catch (Exception ex)
            {
                if (ex.HResult == RPC_S_SERVER_UNAVAILABLE)
                {
                    ResetAfterLostBackground();
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        #region MediaPlayer ReceivedEvent Handler

        private void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {

            AudioTaskStartedMessage backgroundAudioTaskStartedMessage;
            if (MessageService.TryParseMessage(e.Data, out backgroundAudioTaskStartedMessage))
            {
                // StartBackgroundAudioTask is waiting for this signal to know when the task is up and running
                // and ready to receive messages
                Debug.WriteLine("AudioPlayerTask started");
                backgroundAudioTaskStarted.Set();
                _isAudioPlayerTaskRunning = true;
            }
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(e.Data);
            }

        }

        #endregion

        public void Dispose()
        {
            if (backgroundAudioTaskStarted != null)
            {
                backgroundAudioTaskStarted.Dispose();
                backgroundAudioTaskStarted = null;
            }
            if (IsAudioPlayerTaskRunning)
            {
                RemoveMediaPlayerEventHandlers();
                ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.BackgroundTaskState, BackgroundTaskState.Running.ToString());
            }

        }
    }
}
