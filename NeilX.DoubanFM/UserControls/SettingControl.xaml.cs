using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
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
    public sealed partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            this.InitializeComponent();
        }

        public async void SetDownloadLocation()
        {
            FolderPicker picker = new FolderPicker()
            {
                ViewMode = PickerViewMode.List
            };
            picker.FileTypeFilter.Add("*");
            var folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
               // _playlistContentProvider.AddFolder(folder);
            }
        }
    }
}
