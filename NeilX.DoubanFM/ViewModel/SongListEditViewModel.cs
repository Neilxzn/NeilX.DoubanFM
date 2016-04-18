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
    public class SongListEditViewModel:ViewModelBase
    {
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

        public void UpdateInfo()
        {
            LocalDataService.UpdateSongList(SelectList);
        }

        public void Cancel()
        {

        }
        
    }
}
