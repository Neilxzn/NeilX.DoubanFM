using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kfstorm.LrcParser;
using NeilX.DoubanFM.MusicPlayer;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Windows.Storage;

namespace NeilX.DoubanFM.Services.LyricService
{
    public class LyricsService : ObservableObject, ILyricsService
    {
        private ListBox _listBox;
        private TrackInfo _currentTrack;
        public TrackInfo CurrentTrack
        {
            get
            {
                return _currentTrack;
            }

            set
            {
                if (Set(ref _currentTrack, value))
                    OnCurrentTrackChanged(value);
            }
        }

        private ILrcFile _lyricModel;
        public ILrcFile LyricModel
        {
            get
            {
                return _lyricModel;
            }

            set
            {
                Set(ref _lyricModel, value);
            }
        }


        private TimeSpan _position;
        public TimeSpan Position
        {
            get
            {
                return _position;
            }

            set
            {
                if (Set(ref _position, value))
                {
                    OnPositionChange(value);
                }

            }
        }



        public void SetLyricsListBox(ListBox listBox)
        {
            _listBox = listBox;
        }

        private void TryAnalyzeLyrics(string lyrics)
        {
            if (!string.IsNullOrEmpty(lyrics))
            {
                try
                {
                    LyricModel = LrcFile.FromText(lyrics);
                }
                catch { }
            }
        }



        private async void OnCurrentTrackChanged(TrackInfo value)
        {
            if (string.IsNullOrEmpty(value?.Lyric))
            {
                //TODO analyse Lyric url is a local url or a web file,to get 

                var file = await StorageFile.GetFileFromPathAsync(value.Lyric);
                var filetext = await FileIO.ReadTextAsync(file);
                LyricModel = LrcFile.FromText(filetext);
            }
        }

        private void OnPositionChange(TimeSpan position)
        {
            var lyrics = LyricModel;
            if (lyrics != null && lyrics.Lyrics != null)
            {
                var selected = lyrics.BeforeOrAt(position);
                if (selected != null)
                {
                    _listBox.SelectedItem = selected;
                    var display = lyrics.Lyrics.SkipWhile(o => o != selected).Skip(3).FirstOrDefault();
                    if (display != null)
                        _listBox.ScrollIntoView(display);
                    else
                        _listBox.ScrollIntoView(selected);
                }
            }
        }
    }
}
