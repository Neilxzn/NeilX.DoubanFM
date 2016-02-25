using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public class UpdatePlaylistMessage
    {
        public UpdatePlaylistMessage(IList<TrackInfo> tracks)
        {
            this.Tracks = tracks;
        }

        [DataMember]
        public IList<TrackInfo> Tracks;
    }
}
