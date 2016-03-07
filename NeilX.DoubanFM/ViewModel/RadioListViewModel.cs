using GalaSoft.MvvmLight;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.ViewModel
{
    public class RadioListViewModel : ViewModelBase
    {
        private ChannelGroup _myFM;
        private ChannelGroup _artistFM;
        private ChannelGroup _singleSongFM;
        private ChannelGroup _yearsFM;
        private ChannelGroup _styleSongFM;
        private ChannelGroup _moodFM;
        private ChannelGroup _brandFM;

        public ChannelGroup MyFM
        {
            get { return _myFM; }
            set
            {
                Set(ref _myFM, value);
            }
        }
        public ChannelGroup ArtistFM
        {
            get { return _artistFM; }
            set
            {
                Set(ref _artistFM, value);
            }
        }
        public ChannelGroup SingleSongFM
        {
            get { return _singleSongFM; }
            set
            {
                Set(ref _singleSongFM, value);
            }
        }
        public ChannelGroup YearsFM
        {
            get { return _yearsFM; }
            set
            {
                Set(ref _yearsFM, value);
            }
        }
        public ChannelGroup StyleSongFM
        {
            get { return _styleSongFM; }
            set
            {
                Set(ref _styleSongFM, value);
            }
        }
        public ChannelGroup MoodFM
        {
            get { return _moodFM; }
            set
            {
                Set(ref _moodFM, value);
            }
        }
        public ChannelGroup BrandFM
        {
            get { return _brandFM; }
            set
            {
                Set(ref _brandFM, value);
            }
        }


        public RadioListViewModel()
        {
            InitChannelGroup();
        }

        private async void InitChannelGroup()
        {
            var channelground = await DoubanFMService.GetRecommendedChannels();
            if (channelground!=null)
            {
                MyFM = channelground[0];
                ArtistFM = channelground[1];
                SingleSongFM = channelground[2];
                YearsFM = channelground[3];
                StyleSongFM = channelground[4];
                MoodFM = channelground[5];
                BrandFM = channelground[6];
                
            }
            else
            {
                //TODO
            }

        }




    }
}
