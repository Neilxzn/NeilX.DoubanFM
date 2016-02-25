using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    /// <summary>
    /// MessageService makes it easy to send strongly typed messages
    /// between the foreground and background processes.
    /// </summary>
    /// <remarks>
    /// JSON is used as the underlying serialization mechanism,
    /// but you don't need to know JSON formatting to create new
    /// messages.
    /// 
    /// See some of the related Message implementations which are
    /// simple data objects serialized through the standard DataContract
    /// interface.
    /// </remarks>
    public static class MessageService
    {
        // The underlying BMP methods can pass a ValueSet. MessageService
        // relies on this to pass a type and body payload.
        const string MessageType = "MessageType";
        const string MessageBody = "MessageBody";

        public static void SendMessageToForeground<T>(T message)
        {
            var payload = new ValueSet();
            payload.Add(MessageType, typeof(T).FullName);
            payload.Add(MessageBody, JsonHelper.ToJson(message));
            BackgroundMediaPlayer.SendMessageToForeground(payload);
        }
    
        public static void SendMessageToBackground<T>(T message)
        {
            var payload = new ValueSet();
            payload.Add(MessageType, typeof(T).FullName);
            payload.Add(MessageBody, JsonHelper.ToJson(message));
            BackgroundMediaPlayer.SendMessageToBackground(payload);
        }

        public static bool TryParseMessage<T>(ValueSet valueSet, out T message)
        {
            object messageTypeValue;
            object messageBodyValue;

            message = default(T);

            // Get message payload
            if (valueSet.TryGetValue(MessageType, out messageTypeValue)
                && valueSet.TryGetValue(MessageBody, out messageBodyValue))
            {
                // Validate type
                if ((string)messageTypeValue != typeof(T).FullName)
                {
                    Debug.WriteLine("Message type was {0} but expected type was {1}", (string)messageTypeValue, typeof(T).FullName);
                    return false;
                }

                message = JsonHelper.FromJson<T>(messageBodyValue.ToString());
                return true;
            }

            return false;
        }
    }
}
