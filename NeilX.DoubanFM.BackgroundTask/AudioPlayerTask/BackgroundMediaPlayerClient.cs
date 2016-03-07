using NeilX.DoubanFM.MusicPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.BackgroundTask
{
    public sealed class BackgroundMediaPlayerClient
    {
       public  void AttachMessageListener()
        {

        }
        void DetachMessageListener()
        {

        }
        void OnMessageReceivedFromBackground()
        {

        }

        void SendMessageToForeground<T>(T msg)
        {
            MessageService.SendMessageToForeground(msg);
        }
    }
}
