using NeilSoft.UWP;
using NeilX.DoubanFM.MusicPlayer.Controller;
using NeilX.DoubanFM.MusicPlayer.Messages;
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
        
        public MusicPlayerClientProxy Proxy { get; set; }
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
            string key = valueset[MessageService.MessageId] as string;
            if (key ==MessageService.BackgroundMediaPlayerActivatedMessageKey)
            {
                Proxy = new MusicPlayerClientProxy(this );
                PlayerActivated(this, null);
            }
            else if (key == MessageService.BackgroundMediaPlayerUserMessageKey)
            {
                MessageType type =EnumHelper.Parse<MessageType>(valueset[MessageService.MessageType] as string);
                MessageReceived(this, new MessageReceivedEventArgs(type, valueset[MessageService.MessageContent] as string));
            }
        }

    }


}
