using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NeilX.DoubanFM.Common;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.ViewModel
{
    public class SongListViewModel:ViewModelBase, IVMNavigate<SongList>
    {
        public static readonly Guid Token = Guid.NewGuid();
        private MusicSongViewModel _selectSong;

        public MusicSongViewModel SelectSong
        {
            get { return _selectSong; }
            set
            {
                Set(ref _selectSong, value);
            }
        }

        private SongList _selectList;


        public SongList SelectList
        {
            get { return _selectList; }
            set
            {
                if (Set(ref _selectList, value))
                {
                   
                }
            }
        }

        private ObservableCollection<MusicSongViewModel> _listMusicSongs;

        public ObservableCollection<MusicSongViewModel> ListMusicSongs
        {
            get
            {
                return _listMusicSongs ?? new ObservableCollection<MusicSongViewModel>();
            }
            set
            {
                Set(ref _listMusicSongs, value);
            }
        }

        public void PlayListSongs()
        {
            if (ListMusicSongs == null && ListMusicSongs.Count < 1) return;
            List<Song> songs = ListMusicSongs.Select(o => o.Song).ToList();
            ViewModelLocator.Instance.Main.PlayerSession.SetPlaylist(songs, songs[0]);
        }

        public void DelectSongLits()
        {
            LocalDataService.DelectSongList(SelectList);

        }

        public void OnNavigatedTo(SongList t)
        {
            if (SelectList == t) return;
            var allSongs = ViewModelLocator.Instance.MyMusicVM.AllMusicSongs;
            SelectList = t;
            ListMusicSongs= new ObservableCollection<MusicSongViewModel>(
                from s in allSongs
                where s.Song.ListId== t.Id
                select new MusicSongViewModel()
                {
                    Song = s.Song
                });
        }

        public void OnNavigatedFrom(SongList t)
        {
            
        }
    }
}
