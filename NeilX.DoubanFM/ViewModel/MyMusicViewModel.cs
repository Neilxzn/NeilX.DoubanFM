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
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Threading;
using Windows.UI.Xaml.Input;

namespace NeilX.DoubanFM.ViewModel
{
    public class MusicSongViewModel : ViewModelBase
    {
        private Song _song;

        public Song Song
        {
            get { return _song; }
            set
            {
                Set(ref _song, value);
            }
        }

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                Set(ref _index, value);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        public Action<MusicSongViewModel> PlayMethod { get; set; }
        public void PlayLocalSong()
        {
            PlayMethod?.Invoke(this);
        }

    }

    public class MyMusicViewModel : ViewModelBase
    {
        private List<Song> _allLocalSongs;

        private ObservableCollection<MusicSongViewModel> _allMusicSongs;

        public ObservableCollection<MusicSongViewModel> AllMusicSongs
        {
            get
            {
                return _allMusicSongs ?? new ObservableCollection<MusicSongViewModel>();
            }
            set
            {
                Set(ref _allMusicSongs, value);
            }
        }


        private ObservableCollection<SongList> _allSongList;

        public ObservableCollection<SongList> AllSongList
        {
            get
            {
                return _allSongList ?? new ObservableCollection<SongList>();
            }
            set
            {
                Set(ref _allSongList, value);
            }
        }





        public MyMusicViewModel()
        {
            InitialAllLocalSongs();
            InitialSongList();
        }



        public void PlayeAllLocalSongs()
        {
            if (_allLocalSongs != null && _allLocalSongs.Count >= 1)
            {
                IList<Song> list = _allLocalSongs.ToList();
                ServiceLocator.Current.GetInstance<PlayerSessionService>().SetPlaylist(list, list[0]);

            }
        }

        public async void AddSongList()
        {
            SongList list = new SongList()
            {
                Name="播放列表测试",
                BuildTime=DateTime.Now
            };
            LocalDataService.AddSongList(list);
            InitialSongList();
        }




        private async void InitialAllLocalSongs()
        {
            List<Song> oldsongs = await LocalDataService.GetLocalAllSongsAsync(false);
            _allLocalSongs = oldsongs;
            AllMusicSongs = oldsongs != null ? new ObservableCollection<MusicSongViewModel>(
                from s in oldsongs
                select new MusicSongViewModel()
                {
                    Index = oldsongs.IndexOf(s) + 1,
                    Song = s,
                    PlayMethod = PlayLocalSong
                }) : null;
            await Task.Run(async () =>
            {
                List<Song> songs = await LocalDataService.GetLocalAllSongsAsync(true);
                if (oldsongs == null || !(songs != null && oldsongs.Count == songs.Count && oldsongs?.Count(o => !songs.Contains(o)) == 0))
                {
                    await DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                         () =>
                         {
                             AllMusicSongs = songs != null ? new ObservableCollection<MusicSongViewModel>(
                                    from s in songs
                                    select new MusicSongViewModel()
                                    {
                                        Index = oldsongs.IndexOf(s) + 1,
                                        Song = s,
                                        PlayMethod = PlayLocalSong
                                    }) : null;
                             _allLocalSongs = songs;
                         });

                }
            });

        }
        private async void InitialSongList()
        {
            List<SongList> list = await LocalDataService.GetAllSongListsAsync();
            AllSongList = list != null ? new ObservableCollection<SongList>(list) : null;
        }

        private void PlayLocalSong(MusicSongViewModel musicSong)
        {

        }


    }
}
