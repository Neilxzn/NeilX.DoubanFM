using NeilSoft.UWP;
using NeilX.DoubanFM.Common;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Core.LocalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace NeilX.DoubanFM.Services
{
    public class LocalDataService
    {



        public static async Task<List<Song>> GetLocalAllSongsAsync(bool isUpdate)
        {
            DataAccess dataaccess = new DataAccess();
            List<Song> songs;
            if (isUpdate)
            {
                songs = await LocalSongHelper.GetLocalSongsAsync();
                dataaccess.InsertSongs(songs);
            }
            else
            {
                songs = dataaccess.GetAllSongs();
            }
            return songs?.OrderBy(o => o.Title).ToList();
        }
        public static async Task<List<SongList>> GetAllSongListsAsync()
        {
            return await Task.Run(() =>
            {
                DataAccess dataaccess = new DataAccess();
                return dataaccess.GetAllSongLists();
            });
        }


        public static void AddSongList(SongList list)
        {
            DataAccess dataaccess = new DataAccess();
            dataaccess.InsertSongList(list);
        }

        public static void AddSongToSongList(Song newSong,SongList list)
        {
            newSong.ListId = list.Id;
            DataAccess dataaccess = new DataAccess();
            dataaccess.UpdateSong(newSong);
        }
        

        public static void DelectSong(Song song)
        {

        }

        public static void DelectSongList(SongList list)
        {
            DataAccess dataaccess = new DataAccess();
            var songs = dataaccess.GetAllSongs();
            foreach (Song song in songs)
            {
                if (song.ListId ==list.Id)
                {
                    song.ListId = -1;
                }
            }
            dataaccess.UpdateAllSong(songs);
            dataaccess.DelectSongList(list);
        }

        public static void UpdateSongList(SongList list)
        {
            DataAccess dataaccess = new DataAccess();
            dataaccess.UpdateSongList(list);
        }

    }
}
