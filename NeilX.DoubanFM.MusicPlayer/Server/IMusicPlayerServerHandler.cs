using NeilX.DoubanFM.MusicPlayer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Server
{
    interface IMusicPlayerServerHandler
    {
        void OnActivated(MusicPlayerServer mediaPlayer);
        void OnReceiveMessage(MessageType Type,string messege);
        void OnCanceled();
    }
}
