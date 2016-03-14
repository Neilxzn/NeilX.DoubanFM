using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.BackgroundTask.AudioPlayer
{
    interface IBackgroundMediaPlayerServerHandler
    {
        void OnActivated(BackgroundMediaPlayerServer mediaPlayer);
        void OnReceiveMessage(string tag,string messege);
        void OnCanceled();
    }
}
