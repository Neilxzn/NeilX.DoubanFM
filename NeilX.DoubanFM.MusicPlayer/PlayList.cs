using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer
{
    class PlayList
    {
        private const string FileName = "playlist.dat";

        private const string PATH_PlayerState = "player_state.json";

        private const string PlayListFileName = "playlist.dat";
        public TrackInfo[] TrackLists
        {
            get;
            set;
        }
        
    }
}
