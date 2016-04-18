using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Core.LocalData
{
    public class DataAccess
    {
        static DataAccess()
        {
            DbContext.Instance.Init();
        }
        #region Song
        public int InsertSong(Song song)
        {
            int result = 0;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result = db.Insert(song);
            }
            return result;
        }

        public int InsertSongs(List<Song> songs,bool _isChecked=true)
        {
            int result = 0;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                if (_isChecked)
                {
                    var list = db.Table<Song>().ToList();
                    foreach (var item in list)
                    {
                        Song rs = songs.Where(o => o.LocalFileId == item.LocalFileId).FirstOrDefault();
                        if (rs != null)
                        {
                            songs[songs.IndexOf(rs)] = item;
                        }
                    }
                    db.DeleteAll<Song>();
                    result = db.InsertAll(songs);
                }
                else
                {
                    result = db.InsertAll(songs);
                }
               
            }
            return result;
        }

       



        public void DelectSong(Song song)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Delete<Song>(song);
            }
        }



        public void DelectSong(int  id)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Delete<Song>(id);
            }
        }

        public List<Song> GetAllSongs()
        {
            List<Song> result;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result= db.Table<Song>().ToList();
            }
            return result;
        }

        public List<Song> GetSongsByListId(int listId)
        {
            List<Song> result;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result = db.Table<Song>().Where(o=>o.ListId==listId)?.ToList();
            }
            return result;
        }

        public void UpdateSong(Song song)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Update(song);
            }
        }
        public void UpdateAllSong(List<Song> songs)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.UpdateAll(songs);
            }
        }
        #endregion

        #region SongList
        public int InsertSongList(SongList list)
        {
            int result = 0;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result = db.Insert(list);
            }
            return result;
        }

        public void DelectSongList(int id)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Delete<SongList>(id);
                db.Table<Song>().Where(o => o.ListId == id).Select(o=>o.ListId=0);//todo
            }
        }

        public void DelectSongList(SongList list)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Delete<SongList>(list.Id);
            }
        }

        public List<SongList> GetAllSongLists()
        {
            List<SongList> result;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result = db.Table<SongList>().ToList();
            }
            return result;
        }

        public void UpdateSongList(SongList songList)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
                db.Update(songList);
            }
        }
        #endregion

    }
}
