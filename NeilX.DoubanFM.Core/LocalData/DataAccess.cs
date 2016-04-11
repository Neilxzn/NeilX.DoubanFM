using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Core.LocalData
{
    public class DataAccess
    {
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

        public IList<Song> GetSongsByListId(int listId)
        {
            IList<Song> result;
            using (var db = DbContext.Instance.GetDbConnection())
            {
                result = db.Table<Song>().Where(o=>o.ListId==listId).ToList();
            }
            return result;
        }

        public void UpdateSongListId(int songId,int listId)
        {
            using (var db = DbContext.Instance.GetDbConnection())
            {
               
            }
        }
        #endregion


    }
}
