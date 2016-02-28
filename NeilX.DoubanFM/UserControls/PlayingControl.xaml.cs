using Kfstorm.LrcParser;
using NeilX.DoubanFM.MusicPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NeilX.DoubanFM.UserControls
{
    public sealed partial class PlayingControl : UserControl
    {

        private ILrcFile _lyricModel;

        public ILrcFile LyricModel { get; set; }

        public PlayingControl()
        {
            this.InitializeComponent();
        }




        public TrackInfo CurrentTrack
        {
            get { return (TrackInfo)GetValue(CurrentTrackProperty); }
            set
            {
                OnCurrentTrack(value);
                SetValue(CurrentTrackProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentTrack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTrackProperty =
            DependencyProperty.Register("CurrentTrack", typeof(TrackInfo), typeof(PlayingControl), new PropertyMetadata(DependencyProperty.UnsetValue));




        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set
            {
                OnPositionChanged(value);
                SetValue(PositionProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(TimeSpan), typeof(PlayingControl), new PropertyMetadata(TimeSpan.Zero));

        private void OnPositionChanged(TimeSpan position)
        {
            var lyrics = _lyricModel;
            if (lyrics != null && lyrics.Lyrics != null)
            {
                var selected = lyrics.BeforeOrAt(position);
                if (selected != null)
                {
                    lb_Lyrics.SelectedItem = selected;
                    var display = lyrics.Lyrics.SkipWhile(o => o != selected).Skip(3).FirstOrDefault();
                    if (display != null)
                        lb_Lyrics.ScrollIntoView(display);
                    else
                        lb_Lyrics.ScrollIntoView(selected);
                }
            }
        }

        private async void OnCurrentTrack(TrackInfo value)
        {
            if (!string.IsNullOrEmpty(value?.Lyric))
            {
                //TODO analyse Lyric url is a local url or a web file,to get 
                // var file = await StorageFile.GetFileFromPathAsync(value.Lyric));

                var file = await KnownFolders.MusicLibrary.GetFileAsync("12.lrc");// StorageFile.GetFileFromApplicationUriAsync(new Uri( value.Lyric));
                var filetext = await FileIO.ReadTextAsync(file);
                _lyricModel = LrcFile.FromText(filetext);
                lb_Lyrics.ItemsSource = _lyricModel.Lyrics;
            }
        }

    }
}
