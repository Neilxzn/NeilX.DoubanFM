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
        void OnReceiveMessage(string tag,string messege);
        void OnCanceled();
    }
}
