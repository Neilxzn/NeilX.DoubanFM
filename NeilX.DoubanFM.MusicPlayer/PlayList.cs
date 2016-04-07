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


        public IList<Song> TrackLists
        {
            get;
            set;
        }

        public PlayList(IList<Song> songs)
        {
            TrackLists = songs;
            CurrentIndex = 0;
        }

        public PlayList()
        {
            CurrentIndex = 0;
        }

        public Song GetCurrentTrack()
        {
            if (TrackLists!=null&&CurrentIndex < TrackLists.Count)
            {
                return TrackLists[CurrentIndex];
            }
            return null;
        }

        public void CheckCurrentTrack(Song song)
        {

            if (TrackLists == null)
            {
                TrackLists = new List<Song>() { song };
            }
            else if (!TrackLists.Contains(song))
            {
                TrackLists.Add(song);
            }
            CurrentIndex = TrackLists.IndexOf(song);
        }


        public Song MovePrevious()
        {
            if (PlayMode == PlayMode.RepeatOne)
            {
                return TrackLists[this.CurrentIndex];
            }
            if (PlayMode == PlayMode.RepeatAll)
            {
                this.CurrentIndex--;
                if (this.CurrentIndex < 0)
                {
                    this.CurrentIndex = this.TrackLists.Count - 1;
                }
                return TrackLists[CurrentIndex];
            }
            if (PlayMode == PlayMode.List)
            {
                CurrentIndex--;
                if (CurrentIndex < TrackLists.Count)
                {
                    return TrackLists[CurrentIndex];
                }
            }
            if (PlayMode == PlayMode.Shuffle)
            {
                Random random = new Random();
                CurrentIndex = random.Next(TrackLists.Count );
                return TrackLists[CurrentIndex];
            }
            return null;
        }
        public Song MoveNext()
        {

            if (PlayMode == PlayMode.RepeatOne)
            {
                return TrackLists[this.CurrentIndex];
            }
            if (PlayMode == PlayMode.RepeatAll)
            {
                if (++CurrentIndex > TrackLists.Count - 1)
                {
                    CurrentIndex = 0;
                }
                return TrackLists[CurrentIndex];
            }
            if (PlayMode == PlayMode.List && ++CurrentIndex < TrackLists.Count)
            {
                return TrackLists[CurrentIndex];
            }
            if (PlayMode==PlayMode.Shuffle)
            {
                Random random = new Random();
                CurrentIndex = random.Next(TrackLists.Count);
                return TrackLists[CurrentIndex];
            }
            return null;
          
        }


    }
}
