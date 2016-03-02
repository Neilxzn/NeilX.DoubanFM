using Kfstorm.LrcParser;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.Services.LyricService
{
    public interface ILyricsService
    {
        Song CurrentTrack { get; set; }

        ILrcFile LyricModel { get; set; }

        TimeSpan Position { get; set; }

        void SetLyricsListBox(ListBox listBox);

    }
}
