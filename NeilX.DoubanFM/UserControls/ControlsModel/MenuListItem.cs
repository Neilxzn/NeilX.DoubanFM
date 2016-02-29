using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.UserControls.ControlsModel
{
    public enum MenuGotoView
    {
        SearchView,
        RadioListView,
        MyMusicView
    };

    public class MenuListItem : ObservableObject
    {

        private string _name;
        private string _icon;
        private MenuGotoView _gotoView;
        private bool _isEnabled;

        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }

        public string Icon
        {
            get { return _icon; }
            set
            {
                Set(ref _icon, value);
            }
        }

        public MenuGotoView GotoView
        {
            get { return _gotoView; }
            set
            {
                Set(ref _gotoView, value);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                Set(ref _isEnabled, value);
            }
        }
    }
}
