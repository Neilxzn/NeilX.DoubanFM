using GalaSoft.MvvmLight;
using NeilX.DoubanFM.Common;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace NeilX.DoubanFM.ViewModel
{
    public class SongListEditViewModel:ViewModelBase,IVMNavigate<SongList>
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


        public async void ChangeSongListImg()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file!=null)
            {
                string newImgPath ="SongList"+ SelectList.Id + "_Thumbnail";
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                
                await file.CopyAsync(folder, newImgPath,NameCollisionOption.ReplaceExisting);
                //var stream = await file.OpenAsync(FileAccessMode.Read);
                //var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                //await bitmapImage.SetSourceAsync(stream);
                //var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                _selectList.Thumbnail = folder.Path+"\\"+ newImgPath;
                RaisePropertyChanged(nameof(SelectList));
            }
        }

        public void UpdateInfo()
        {
            LocalDataService.UpdateSongList(SelectList);
            ViewModelLocator.Instance.Main.NavigationServiceGoBack();
        }

        public void Cancel()
        {
            ViewModelLocator.Instance.Main.NavigationServiceGoBack();
        }

        public void OnNavigatedTo(SongList t)
        {
            if (SelectList == t) return;
            SelectList = t;
        }

        public void OnNavigatedFrom(SongList t)
        {
            
        }
    }
}
