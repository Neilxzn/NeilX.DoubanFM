using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml;
using NeilX.DoubanFM.MusicPlayer.Controller;
using NeilX.DoubanFM.Services;
using NeilX.DoubanFM.View;

namespace NeilX.DoubanFM.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RadioListViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();

            SimpleIoc.Default.Register<PlayerSessionService>(true);


          //  SimpleIoc.Default.Register(CreateNavigationService);

        }



        public static ViewModelLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (Application.Current.Resources["Locator"] as ViewModelLocator);
                }
                return _instance;
            }
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SearchViewModel SearchVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchViewModel>();
            }
        }

        public RadioListViewModel RadioListVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RadioListViewModel>();
            }
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("MainPage", typeof(MainPage));
            //  navigationService.Configure("PlayerPage", typeof(PlayerPage));
            
            return navigationService;
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}