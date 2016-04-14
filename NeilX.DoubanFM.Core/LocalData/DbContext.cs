using NeilX.DoubanFM.Core.Common;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Core.LocalData
{
    public class DbContext: SingletonProvider<DbContext>
    {
        private const string DbFileName = "LocalData.db";
        private  string DbFilePath;

        public void Init()
        {
            DbFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DbFileName);
            using (SQLiteConnection db=GetDbConnection())
            {
                db.CreateTable<Song>();
                db.CreateTable<SongList>();
            }
        }


        public SQLiteConnection GetDbConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), DbFilePath);
        }
    }
}
