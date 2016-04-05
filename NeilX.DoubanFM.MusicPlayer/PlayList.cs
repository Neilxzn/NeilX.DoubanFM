using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer
{
    public class PlayList
    {
        private const string FileName = "playlist.dat";

        private const string PATH_PlayerState = "player_state.json";

        private const string PlayListFileName = "playlist.dat";

        public int CurrentIndex
        {
            get;
            set;
        }

        public PlayMode PlayMode { get; set; }


        public List<Song> TrackLists
        {
            get;
            set;
        }

        public PlayList(List<Song> songs)
        {
            TrackLists = songs;
        }


        public Song GetCurrentTrack()
        {
            if (TrackLists!=null&&CurrentIndex < TrackLists.Count)
            {
                return TrackLists[CurrentIndex];
            }
            return null;
        }

        public Song MovePrevious()
        {
            if (PlayMode == PlayMode.SingleLoop)
            {
                return TrackLists[this.CurrentIndex];
            }
            if (PlayMode == PlayMode.Loop)
            {
                if (++CurrentIndex > TrackLists.Count - 1)
                {
                    CurrentIndex = 0;
                }
                return TrackLists[CurrentIndex];
            }
            if (PlayMode == PlayMode.Single && ++CurrentIndex < TrackLists.Count)
            {
                return TrackLists[CurrentIndex];
            }
            return null;
        }
        public Song MoveNext()
        {
            if (PlayMode == PlayMode.SingleLoop)
            {
                return TrackLists[this.CurrentIndex];
            }
            if (PlayMode == PlayMode.Loop)
            {
                this.CurrentIndex--;
                if (this.CurrentIndex < 0)
                {
                    this.CurrentIndex = this.TrackLists.Count - 1;
                }
                return TrackLists[CurrentIndex];
            }
            if (PlayMode == PlayMode.Single)
            {
                CurrentIndex--;
                if (CurrentIndex < TrackLists.Count)
                {
                    return TrackLists[CurrentIndex];
                }
            }
            return null;
        }


    }
}
