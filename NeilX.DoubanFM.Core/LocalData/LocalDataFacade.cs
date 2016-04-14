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
    public class LocalDataFacade
    {
        private const string DbFileName = "LocalData.db";
        private readonly string DbFilePath;
        public LocalDataFacade()
        {
            DbFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DbFileName);
        }
        public void TestInsert()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbFilePath))
            {

                db.CreateTable<Song>();

                Song item = new Song("123");
                item.Id = 1;
                item.AlbumUrl = "234";

                db.Insert(item);

            }
        }

        public void TestGet()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbFilePath))

            {

                var list = db.Table<Song>();
                var s= list.FirstOrDefault();
            }
        }
    }
}
