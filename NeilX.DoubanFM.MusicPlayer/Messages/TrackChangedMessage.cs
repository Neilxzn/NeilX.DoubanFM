using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public class TrackChangedMessage
    {


        public TrackChangedMessage(Song song)
        {
            this.Song = song;
        }

        [DataMember]
        public Song Song;
    }
}
