using NeilSoft.UWP;
using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace NeilX.DoubanFM.Common
{
    public class LocalSongHelper
    {
        public async static Task<List<Song>> GetLocalSongsAsync()
        {
            StorageFolder musicFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.MusicLibrary);

            List<string> fileTypeFilter = new List<string>()
            {
                ".mp3",".mp4",".wmv"
            };
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderBySearchRank, fileTypeFilter);
            StorageFileQueryResult queryResult = musicFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> files = await queryResult.GetFilesAsync();
            if (files.Count > 0)
            {
                List<Song> songs = new List<Song>();
                foreach (var file in files)
                {
                    MusicProperties info = await file.Properties.GetMusicPropertiesAsync();
                    if (info == null) break;
                    Song song = new Song("9" + file.GetHashCode().ToString())
                    {
                        AlbumTitle = info.Album,
                        Artist = info.Artist,
                        Kbps = (int?)info.Bitrate,
                        Length = (int)info.Duration.TotalSeconds,
                        PublishTime = (int?)info.Year,
                        Title = info.Title,
                        Url = file.Path,
                        LocalFileId = file.FolderRelativeId,
                        PictureUrl = @"ms-appx:///Assets/Images/m51.jpg"
                    };
                    songs.Add(song);

                }
                return songs;
            }
            return null;

        }

        public static async Task<bool> IsMusicFolderDateModifiedAsync()
        {
            StorageFolder m = KnownFolders.MusicLibrary;
            StorageFolder musicFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.MusicLibrary);
            long oldlastDate = AppSettingsHelper.LoadSetttingFromLocalSettings("LocalDataModifiedDate", 0L);
            long lastDate = (await musicFolder.GetBasicPropertiesAsync()).DateModified.ToUnixTimeSeconds();
            if (oldlastDate != lastDate)
            {
                AppSettingsHelper.SaveSettingToLocalSettings("LocalDataModifiedDate",lastDate);
                AppSettingsHelper.LoadSetttingFromLocalSettings("LocalDataModifiedDate", lastDate);
                return true;
            }
            return false;
        }
    }
}
