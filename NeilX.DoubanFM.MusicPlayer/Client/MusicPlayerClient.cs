using NeilX.DoubanFM.MusicPlayer.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Client
{
    public sealed class MusicPlayerClient
    {
        const int RPC_S_SERVER_UNAVAILABLE = -2147023174; // 0x800706BA
        private const string BackgroundMediaPlayerActivatedMessageKey = @"Activated";
        private const string BackgroundMediaPlayerUserMessageKey = @"UserMessage";
        public IMusicPlayerController Proxy { get; set; }
        public  event EventHandler<object> PlayerActivated;
       public  event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public MusicPlayerClient()
        {
            AttachMessageListener();
        }

        private void AttachMessageListener()
        {
            BackgroundMediaPlayer.MessageReceivedFromBackground += OnMessageReceivedFromBackground;
        }
        private void DetachMessageListener()
        {
            BackgroundMediaPlayer.MessageReceivedFromBackground -= OnMessageReceivedFromBackground;
        }

        private void OnMessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            var valueset = e.Data;
            string key = valueset["MessageId"] as string;
            if (key == BackgroundMediaPlayerActivatedMessageKey)
            {
                Proxy = new MusicPlayerClientProxy(BackgroundMediaPlayer.Current );
                PlayerActivated(this, null);
            }
            else if (key == BackgroundMediaPlayerUserMessageKey)
                MessageReceived(this, new MessageReceivedEventArgs(valueset["MessageTag"] as string, valueset["MessageContent"] as string));
        }
        public void SendMessageToClient(string tag, string message)
        {
            ValueSet valueset = new ValueSet();
            valueset.Add("MessageId", BackgroundMediaPlayerUserMessageKey);
            valueset.Add("MessageTag", tag);
            valueset.Add("MessageContent", message);
            bool failed = true;
            int retryCount = 2;

            while (--retryCount >= 0)
            {
                try
                {
                    BackgroundMediaPlayer.SendMessageToForeground(valueset);
                    failed = false;
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.HResult == RPC_S_SERVER_UNAVAILABLE)
                        ;//do again
                    else
                        throw;
                }
                try
                {
                    BackgroundMediaPlayer.Shutdown();
                    AttachMessageListener();
                }
                catch (Exception ex)
                {
                }
            }
            if (failed)
                throw  new COMException(RPC_S_SERVER_UNAVAILABLE.ToString());
        }
    }


}
