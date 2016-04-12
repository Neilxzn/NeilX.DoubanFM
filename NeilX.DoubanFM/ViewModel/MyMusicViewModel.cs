using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.ViewModel
{
    public class MyMusicViewModel : ViewModelBase
    {

        private ObservableCollection<Song> _alllocalSongs;

        public ObservableCollection<Song> AlllocalSongs
        {
            get
            {
                return _alllocalSongs ?? new ObservableCollection<Song>();
            }
            set
            {
                Set(ref _alllocalSongs, value);
            }
        }





        public MyMusicViewModel()
        {
            InitialAllLocalSongs();
        }






        private async void InitialAllLocalSongs()
        {
            List<Song> oldsongs = await LocalDataService.GetLocalAllSongsAsync(false);
            AlllocalSongs = new ObservableCollection<Song>(oldsongs);
            await Task.Run(async () =>
            {
                List<Song> songs = await LocalDataService.GetLocalAllSongsAsync(true);
                if (oldsongs!=songs)
                {
                    AlllocalSongs = new ObservableCollection<Song>(songs);
                }
            });
          
        }

    }
}
