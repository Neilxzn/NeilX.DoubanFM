using NeilX.DoubanFM.MusicPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Client
{
     public sealed class MessageReceivedEventArgs
    {

        public MessageReceivedEventArgs(MessageType type, string messege)
        {
            this.Type = type;
            this.MessegeContent = messege;
        }

        public MessageType Type { get; set; }
        public string MessegeContent { get; set; }
    }
}
