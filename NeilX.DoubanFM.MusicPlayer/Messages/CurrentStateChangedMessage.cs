using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public class CurrentStateChangedMessage
    {
        [DataMember]
        public string PlayerStateStr { get; set; }
        public CurrentStateChangedMessage(MediaPlayerState state)
        {
            PlayerStateStr = state.ToString();
        }
    }
}
