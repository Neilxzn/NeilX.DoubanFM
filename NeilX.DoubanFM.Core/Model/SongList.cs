using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Core
{
    public class SongList
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public DateTime BuildTime { get; set; }

        public int SongCount { get; set; }


    }
}
