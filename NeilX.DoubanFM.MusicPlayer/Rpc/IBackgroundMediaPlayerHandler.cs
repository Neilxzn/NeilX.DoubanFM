using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Rpc
{
    interface IBackgroundMediaPlayerServerHandler
    {
        void OnActivated(BackgroundMediaPlayerServer mediaPlayer);
        void OnReceiveMessage(string tag,string messege);
        void OnCanceled();
    }
}
