using System;
using Newtonsoft.Json.Linq;

namespace NeilX.DoubanFM.Core
{
    /// <summary>
    /// Helper class to parse server response to readable objects
    /// </summary>
    public static class ParserHelper
    {
        /// <summary>
        /// Parses the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        public static Channel ParseChannel(this JToken channel)
        {
            var c= new Channel((int)channel["id"])
            {
                Name = (string) channel["name"],
                Artist = (string)(channel["artist"]??""),
                Description = (string)channel["intro"],
                SongCount = (int)(channel["song_num"]??0),
                CoverUrl = (string)channel["cover"],
                Start = (string)channel["start"],
                // TODO: 'collected'
                // TODO: 'style'
            };
            return c;
        }

        /// <summary>
        /// Parses the song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public static Song ParseSong(this JToken song)
        {
            return new Song((string)song["sid"])
            {
                AlbumUrl = (string)song["album"],
                PictureUrl = (string)song["picture"],
                Ssid = (string)song["ssid"],
                Artist = (string)song["artist"],
                Url = (string)song["url"],
                Company = (string)song["company"],
                Title = (string)song["title"],
                Length = (int)song["length"],
                SubType = (string)song["subtype"],
                PublishTime = (int?)(song["public_time"]),
                Aid = (string)song["aid"],
                Sha256 = (string)song["sha256"],
                Kbps = (int?)(song["kbps"]),
                AlbumTitle = (string)song["albumtitle"],
                Like = (int)song["like"]==0?false:true,
            };
        }

        /// <summary>
        /// Parses an optional object/field.
        /// </summary>
        /// <typeparam name="T">The type of the object/field.</typeparam>
        /// <param name="obj">The JSON token.</param>
        /// <returns>The value of the object/field.</returns>
        public static T ParseOptional<T>(this JToken obj)
        {
            try
            {
                if (obj == null)
                {
                    return default(T);
                }
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (FormatException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// If the token is an instance of JArray, then return it directly. Otherwise return an empty JArray.
        /// </summary>
        /// <param name="obj">The JSON token.</param>
        /// <returns>The JArray object.</returns>
        public static JArray GetArrayOrEmpty(this JToken obj)
        {
            var array = obj as JArray;
            if (array != null) return array;
            return new JArray();
        }
    }
}
