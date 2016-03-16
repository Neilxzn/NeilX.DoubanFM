using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Rpc
{
     public sealed class MessageReceivedEventArgs
    {

        public MessageReceivedEventArgs(string tag, string messege)
        {
            this.Tag = tag;
            this.MessegeContent = messege;
        }

        public string Tag { get; set; }
        public string MessegeContent { get; set; }
    }
}
