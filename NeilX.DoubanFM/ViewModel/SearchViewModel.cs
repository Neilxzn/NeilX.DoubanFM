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
    public class SearchViewModel : ViewModelBase
    {

        private string _query;

        public string Query
        {
            get { return _query; }
            set
            {
                Set(ref _query, value);
            }
        }


        private ObservableCollection<Channel> _channels;

        public ObservableCollection<Channel> Channels
        {
            get
            {
                return _channels??new ObservableCollection<Channel>();
            }
            set
            {
                Set(ref _channels, value);
            }
        }

        private ObservableCollection<string> _histryQuery;

        public ObservableCollection<string> HistryQuery
        {
            get { return _histryQuery; }
            set
            {
                Set(ref _histryQuery, value);
            }
        }



        public SearchViewModel()
        {

        }

        public async void SearchChannel()
        {
            Channels = new ObservableCollection<Channel>(await DoubanFMService.SearchChannelAsync(Query, 0, 20));
        }

        public async void SelectTheChannle(object sender, ItemClickEventArgs e)
        {
            var channel = e.ClickedItem as Channel;
            List<Song>  songs=await   DoubanFMService.GetSongsFromChannel(channel);
            ViewModelLocator.Instance.Main.PlayerSession.SetPlaylist(songs, songs[0]);
        }


        private void InitialHistryQuery()
        {

        }

    }
}
