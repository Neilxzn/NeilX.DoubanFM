using NeilX.DoubanFM.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public class UpdatePlaylistMessage
    {
        public UpdatePlaylistMessage(IList<Song> tracks)
        {
            this.Tracks = tracks;
        }

        [DataMember]
        public IList<Song> Tracks;
    }
}
