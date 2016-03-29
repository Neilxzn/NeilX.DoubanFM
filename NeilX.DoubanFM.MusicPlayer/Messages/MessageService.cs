using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    public static class MessageService
    {
        const int RPC_S_SERVER_UNAVAILABLE = -2147023174; // 0x800706BA
        public const string MessageId = "MessageId";
        public const string MessageType = "MessageType";
        public const string MessageContent = "MessageContent";

        public const string BackgroundMediaPlayerActivatedMessageKey = @"Activated";
        public const string BackgroundMediaPlayerUserMessageKey = @"UserMessage";


        public static void SendMessageToServer<T>(T message)
        {
            ValueSet valueset = new ValueSet();
            valueset.Add(MessageId, BackgroundMediaPlayerUserMessageKey);
            valueset.Add(MessageType, typeof(T).Name);
            valueset.Add(MessageContent, JsonHelper.ToJson(message));
            bool failed = true;
            int retryCount = 2;

            while (--retryCount >= 0)
            {
                try
                {
                    BackgroundMediaPlayer.SendMessageToBackground(valueset);
                    failed = false;
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.HResult == RPC_S_SERVER_UNAVAILABLE)
                        BackgroundMediaPlayer.SendMessageToBackground(valueset);
                    else
                        throw;
                }
                try
                {
                    BackgroundMediaPlayer.Shutdown();
                   //  AttachMessageListener();
                }
                catch (Exception ex)
                {
                }
            }
            if (failed)
                throw new COMException(RPC_S_SERVER_UNAVAILABLE.ToString());
        }

        public static void SendMessageToClient<T>(T message)
        {
            ValueSet valueset = new ValueSet();
            valueset.Add(MessageId, BackgroundMediaPlayerUserMessageKey);
            valueset.Add(MessageType, typeof(T).Name);
            valueset.Add(MessageContent, JsonHelper.ToJson(message));
            BackgroundMediaPlayer.SendMessageToForeground(valueset);
        }
    }

   
}
