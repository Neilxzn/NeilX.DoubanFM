using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
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
    public class SongListViewModel:ViewModelBase
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

        public void EditListInfo()
        {
            
            Messenger.Default.Send(new NotificationMessage("GotoEditView"), Token);
            ServiceLocator.Current.GetInstance<SongListEditViewModel>().SelectList = SelectList;

        }

        public void DelectSongLits()
        {
            LocalDataService.DelectSongList(SelectList);
            Messenger.Default.Send(new NotificationMessage("Update"), Token);
            ServiceLocator.Current.GetInstance<MyMusicViewModel>().ReflashData();
        }
    }
}
