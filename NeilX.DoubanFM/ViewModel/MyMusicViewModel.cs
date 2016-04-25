using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using NeilX.DoubanFM.View;
using Windows.UI.Xaml.Controls;
using NeilX.DoubanFM.View.Flyout;
using MicroMsg.sdk;
using System.Diagnostics;

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



        public void PlayLocalSong()
        {
            if (Song != null)
            {
                ServiceLocator.Current.GetInstance<PlayerSessionService>().AddSongToPlaylist(Song);
            }
        }

        public void AddSongToSongList()
        {
            IEnumerable<SongList> list = ViewModelLocator.Instance.MyMusicVM.AllSongList;
            ViewModelLocator.Instance.NavigationService.ShowCenterFlyout(new AddSongToSongListFlyout(list, Song));
        }

        public void LookSongInfo()
        {
            ViewModelLocator.Instance.NavigationService.ShowRightFlyout(new SongInfoFlyout(this));
        }


        public async void ShareSong()
        {
            try
            {
                var scene = SendMessageToWX.Req.WXSceneTimeline;
                var message = new WXTextMessage
                {
                    Title = "Sharing a text title!",
                    Text = "This is text content",
                    Description = "This is a text message.这是一个文本消息。",
                    ThumbData = null
                };
                SendMessageToWX.Req req = new SendMessageToWX.Req(message, scene);
                IWXAPI api = WXAPIFactory.CreateWXAPI("[YOUR APP ID]");
                var isValid = await api.SendReq(req);
            }
            catch (WXException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void DelectThisSong()
        {
            Action action = () =>
            {
                LocalDataService.DelectSong(Song);
                ViewModelLocator.Instance.MyMusicVM.ReflashData();
            };
            ViewModelLocator.Instance.NavigationService.ShowCenterFlyout(new ConfirmFlyout(action));
        }

    }

    public class MyMusicViewModel : ViewModelBase
    {
        public static readonly Guid Token = Guid.NewGuid();
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


        private object _selectSong;

        public object SelectSong
        {
            get { return _selectSong; }
            set
            {
                if (_selectSong != value)
                {
                    if (_selectSong != null)
                    {
                        ((MusicSongViewModel)_selectSong).IsSelected = false;
                        RaisePropertyChanged(nameof(SelectSong));
                    }
                    _selectSong = value;
                    if (_selectSong != null)
                        ((MusicSongViewModel)_selectSong).IsSelected = true;
                    RaisePropertyChanged(nameof(SelectSong));
                }
            }
        }








        public MyMusicViewModel()
        {
            SelectSong = null;
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

        public void AddNewSongList(string listName)
        {

            SongList list = new SongList()
            {
                Name = listName,
                BuildTime = DateTime.Now,
                Thumbnail = "ms-appx:///Assets/Images/m51.jpg"
            };
            LocalDataService.AddSongList(list);
            ReflashData(false);
        }



        public void ReflashData(bool isBoth = true)
        {
            if (isBoth)
            {
                InitialAllLocalSongs();
                InitialSongList();
            }
            else
            {
                InitialSongList();
            }

        }

        public override void Cleanup()
        {

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
                    Song = s
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
                                        Song = s
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
    }
}
