using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NeilX.DoubanFM.ViewModel;
using NeilX.DoubanFM.MusicPlayer;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI;
using NeilX.DoubanFM.UserControls;
using NeilX.DoubanFM.Core;
using Kfstorm.LrcParser;
using Windows.Storage.Pickers;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace NeilX.DoubanFM.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel Main => (MainViewModel)DataContext;


        public MainPage()
        {
            this.InitializeComponent();
            InitializeMessenger();

            Unloaded += (sender, e) => Messenger.Default.Unregister(this);


        }
        #region Helper methods
        private void InitializeMessenger()
        {
            Messenger.Default.Register<string>(this, "MainPage", OpenSettingView);
        }

        #endregion
        
        #region Update UI Methods
        private void OpenSettingView(string msg)
        {
            if (msg != "OpenSettingView") return;
            SettingsFlyout settings = new SettingsFlyout();
            
            // optionally change header and content background colors away from defaults (recommended)
            //settings.Background = new SolidColorBrush(Colors.Red);
            settings.HeaderBackground = new SolidColorBrush(Colors.Orange);
            settings.Content = new SettingControl();
            
            // open it
            settings.Show();
        }
        #endregion
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            //var session = new Session(new ServerConnection("02646d3fb69a52ff072d47bf23cef8fd", "cde5d61429abcd7c", "radio_iphone", "100", new Uri("http://www.douban.com/mobile/fm"), Guid.NewGuid().ToString("N")));
            //var player = new Player(session);
            //var discovery = new Discovery(session);
            //var channelGroups = await discovery.GetRecommendedChannels();
            //var newChannel = channelGroups[1].Channels[0];
            //await player.ChangeChannel(newChannel);
            //var currentSong = player.CurrentSong;
            List<TrackInfo> tracks = new List<TrackInfo>();
            TrackInfo track = new TrackInfo();
            //track.CoverThumbnail = currentSong.PictureUrl;
            //track.Source = new Uri( currentSong.Url);
            //track.Title = currentSong.Title;
            //track.Artist = currentSong.Artist;
            //track.Duration = TimeSpan.FromSeconds(currentSong.Length);
            //tracks.Add(track);
            track.Title = "微光";
            track.Artist = "张信哲";
            track.Source = new Uri(@"ms-appx:///SampleMedias/ring4.mp3");
            track.Lyric = @"ms-appx:///SampleMedias/ring4.lrc";
            track.Duration = TimeSpan.FromSeconds(282);
            track.CoverThumbnail  = @"ms-appx:///SampleMedias/ring4.jpg";
            tracks.Add(track);
            Main.PlayerSession.SetPlaylist(tracks,track);
            ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.AppState, AppState.Active.ToString());
        }

     



        //#region test a download method

        //async void Test()
        //{
        //    //List<BannerItem> banners = new List<BannerItem>();
        //    //// await DownLoadHelper.DownLoadSingleFileAsync("http://m6.file.xiami.com/256/23256/1266008391/1771811908_4404186_h.mp3?auth_key=4439d682b7784064d66ab71b3fb64569-1455127200-0-null", await CreateResultFileAsync(1), DownLoadProgress);
        //    //var response = await HttpClientHelper.GetResponseByGetAsync(Constants.GetMusicStartUriStr());
        //    //var jsonString = ChineseWordHelper.GetString(await response.Content.ReadAsStringAsync());
        //    //var jsonobject = JsonObject.Parse(jsonString);
        //    ////var jsonobject = JsonHelper.FromJson<ReponseBaseModel<object>>(jsonString);
        //    //var data = jsonobject.GetNamedObject("data");
        //    //await XiamiService.GetMusicStartDataAsync(banners);
        //    string secret = "9ec4132d14aced0951835ecc3e26f48e";
        //    IDictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("method", "alibaba.xiami.api.rank.songs.get");
        //    parameters.Add("app_key", "23309862");
        //    parameters.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    parameters.Add("format", "json");
        //    parameters.Add("v", "2.0");
        //    parameters.Add("sign_method", "md5");
        //    parameters.Add("type", "newmusic_all");

        //    string result = SignTopRequest(parameters, secret, "md5");
        //    parameters.Add("sign", result);
        //    string TESTURL = "http://gw.api.taobao.com/router/rest?";
        //    foreach (var item in parameters)
        //    {
        //        TESTURL += item.Key + "=" + item.Value + "&";
        //    }
        //    string TEST2 = TESTURL.Substring(0, TESTURL.Length - 1);
        //    var response = await HttpClientHelper.GetResponseByGetAsync(TEST2);
        //    string responseStr = await response.Content.ReadAsStringAsync();
        //}





        //public string SignTopRequest(IDictionary<string, string> parameters, string secret, string signMethod)
        //{
        //    // 第一步：把字典按Key的字母顺序排序
        //    IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
        //    IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

        //    // 第二步：把所有参数名和参数值串在一起
        //    StringBuilder query = new StringBuilder();
        //    if (signMethod == "md5")
        //    {
        //        query.Append(secret);
        //    }
        //    while (dem.MoveNext())
        //    {
        //        string key = dem.Current.Key;
        //        string value = dem.Current.Value;
        //        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        //        {
        //            query.Append(key).Append(value);
        //        }
        //    }
        //    // 第三步：使用MD5加密转化为大写的十六进制
        //    query.Append(secret);
        //    string result = ComputeMD5(query.ToString()).ToUpper();
        //    return result;
        //}
        //string ComputeMD5(string message)
        //{
        //    //可以选择MD5 Sha1 Sha256 Sha384 Sha512
        //    // 创建一个 HashAlgorithmProvider 对象
        //    HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
        //    IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8);
        //    var hashed = objAlgProv.HashData(buffMsg);
        //    string result = CryptographicBuffer.EncodeToHexString(hashed);
        //    return result;
        //}

        //async void DownLoadProgress(DownloadOperation download)
        //{
        //    BackgroundDownloadProgress currentProgress = download.Progress;
        //    double percent = 100;
        //    if (currentProgress.TotalBytesToReceive > 0)
        //    {
        //        percent = currentProgress.BytesReceived * 100 / currentProgress.TotalBytesToReceive;
        //    }
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //    //    dprogress.Value = percent;
        //    });
        //}
        //private async Task<IStorageFile> CreateResultFileAsync(int id)
        //{
        //    IStorageFile resultFile = await KnownFolders.PicturesLibrary.CreateFileAsync(
        //        String.Format(CultureInfo.InvariantCulture, "picture{0}.mp3", id),
        //        CreationCollisionOption.ReplaceExisting);
        //    return resultFile;
        //}
        //private async void Load_Click(object sender, RoutedEventArgs e)
        //{
        //    // Test();//http://m6.file.xiami.com/544/127544/2100268498/1775567274_59681010_h.mp3?auth_key=c1b7c51064ce48cc8cfc6b3c7dba6a92-1455127200-0-null 
        //    Songs songs = await XiamiService.GetResponseAsync<Songs>(Constants.UrlRadioGuess, ServiceType.Radio_Detail);
        //}
        //private void Pause_Click(object sender, RoutedEventArgs e)
        //{
        //    DownLoadHelper.PauseSingleDownload("picture1.mp3");
        //}
        //private void Resume_Click(object sender, RoutedEventArgs e)
        //{
        //    DownLoadHelper.ResumeSingleDownload("picture1.mp3");
        //}
        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    DownLoadHelper.CancelSingleDownLoads("picture1.mp3");
        //}
        //#endregion

     

        private void Hambeger_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //List<TrackInfo> tracks = new List<TrackInfo>();
            //Uri uri = new Uri("ms-appx:///SampleMedias/Ring01.mp3");
            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            //await TryAddTrackInfo(file, tracks);    
            //   InitializeSongs();
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add ("*");
          var file=  await  picker.PickSingleFileAsync();
           
           // var file = await StorageFile.GetFileFromPathAsync(value.Lyric);
            var filetext = await FileIO.ReadTextAsync(file);
             var lyricfile = LrcFile.FromText(filetext);
        }

        async void InitializeSongs()
        {
            List<TrackInfo> songs = new List<TrackInfo>();
            TrackInfo song = new TrackInfo();
            song.Title = "w1";
            song.Artist = "T1";
            song.Source = new Uri( "ms-appx:///SampleMedias/Ring01.mp3");
            song.CoverThumbnail = "ms-appx:///SampleMedias/Ring01.jpg";
            TrackInfo song2 = new TrackInfo();
            song2.Title = "w2";
            song2.Artist = "T2";
            song2.Source = new Uri("ms-appx:///SampleMedias/Ring02.mp3");
            song2.CoverThumbnail = "ms-appx:///SampleMedias/Ring02.jpg";
            TrackInfo song3 = new TrackInfo();
            song3.Title = "w3";
            song3.Artist = "T3";
            song3.Source = new Uri("ms-appx:///SampleMedias/Ring03.mp3");
            song3.CoverThumbnail = "ms-appx:///SampleMedias/Ring02.jpg";
            songs.Add(song);
            songs.Add(song2);
            songs.Add(song3);

 
            foreach (var item in songs)
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(item.Source);
                var info = await file.Properties.GetMusicPropertiesAsync();
                item.Duration = info.Duration;
            }
            Main.PlayerSession.SetPlaylist(songs, song);

        }


        private void SetupTitleBar()
        {
            
            //CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //coreTitleBar.ExtendViewIntoTitleBar = true;
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //     Window.Current.SetTitleBar(TitleGrid);
        }
    }
}
