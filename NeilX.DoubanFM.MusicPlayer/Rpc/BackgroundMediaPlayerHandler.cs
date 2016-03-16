using NeilX.DoubanFM.MusicPlayer.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Rpc
{
    public class BackgroundMediaPlayerHandler : IBackgroundMediaPlayerServerHandler
    {
        private MusicPlayerController _musicPlayerController;


        public void OnActivated(BackgroundMediaPlayerServer mediaPlayer)
        {
            _musicPlayerController = new MusicPlayerController(mediaPlayer);
        }

        public void OnCanceled()
        {
            _musicPlayerController.OnCanceled();
        }

        public void OnReceiveMessage(string tag, string message)
        {
            if (tag =="")
                _musicPlayerController?.OnReceiveMessage(message);
            else
            {
                Debug.WriteLine($"Client Message: {tag}, {message}");
            }
        }
    }
}
