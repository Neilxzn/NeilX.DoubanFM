using GalaSoft.MvvmLight.Messaging;
using Kfstorm.LrcParser;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using NeilX.DoubanFM.Common;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.MusicPlayer;
using NeilX.DoubanFM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NeilX.DoubanFM.View.UserControls
{
    public sealed partial class PlayingControl : UserControl
    {
        #region private fileds
        public static readonly Guid Token = Guid.NewGuid();
        private CanvasBitmap imgbackground;
        private GaussianBlurEffect blurEffect;
        private ScaleEffect scaleEffect;
        private string oldImgPath;
        private Storyboard ellStoryboard;

        #endregion

        #region Construcetor

        public PlayingControl()
        {
            this.InitializeComponent();
            ViewModelLocator.Instance.Main.PlayerSession.IsPlayingChanged += IsPlayingChanged;
            ellStoryboard = Resources["EllStoryboard"] as Storyboard;
        }


        #endregion

        #region IsOpen


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PlayingControl), new PropertyMetadata(false,OnIsOpenPropertyChangeCallBack));

        private static void OnIsOpenPropertyChangeCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayingControl control = d as PlayingControl;
            control.OnIsOpenChanged((bool)e.NewValue);
        }

        void OnIsOpenChanged(bool _isOpen)
        {
            if (_isOpen&&ViewModelLocator.Instance.Main.PlayerSession.IsPlaying)
            {
                ellStoryboard.Resume();
            }
            else
            {
                ellStoryboard.Pause();
            }
        }

        #endregion

        #region CurrentTrack


        public Song CurrentTrack
        {
            get { return (Song)GetValue(CurrentTrackProperty); }
            set
            {
                OnCurrentTrack(value);
                SetValue(CurrentTrackProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentTrackProperty =
            DependencyProperty.Register("CurrentTrack", typeof(Song), typeof(PlayingControl), new PropertyMetadata(DependencyProperty.UnsetValue));
        private async void OnCurrentTrack(Song value)
        {
            ellStoryboard.Stop();
            ellStoryboard.Begin();
            if (!IsOpen||! ViewModelLocator.Instance.Main.PlayerSession.IsPlaying)
            {
                ellStoryboard.Pause();
            }
            string ImgPath = value?.PictureUrl;
            var action = BlurCanvas.RunOnGameLoopThreadAsync(async () =>
            {
                await ReflashBackground(BlurCanvas, ImgPath);
                BlurCanvas.Invalidate();

            });
          //  if (!string.IsNullOrEmpty(value?.Lyric))
         //   {
                //TODO analyse Lyric url is a local url or a web file,to get 
                // var file = await StorageFile.GetFileFromPathAsync(value.Lyric));

                var file = await KnownFolders.MusicLibrary.GetFileAsync("12.lrc");// StorageFile.GetFileFromApplicationUriAsync(new Uri( value.Lyric));
                var filetext = await FileIO.ReadTextAsync(file);
                _lyricModel = LrcFile.FromText(filetext);
                lb_Lyrics.ItemsSource = _lyricModel.Lyrics;
          //  }
        }
        #endregion

        #region Position

        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set
            {
                OnPositionChanged(value);
                SetValue(PositionProperty, value);
            }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(TimeSpan), typeof(PlayingControl), new PropertyMetadata(TimeSpan.Zero));

        private void OnPositionChanged(TimeSpan position)
        {
            var lyrics = _lyricModel;
            if (lyrics != null && lyrics.Lyrics != null)
            {
                var selected = lyrics.BeforeOrAt(position);
                if (selected != null)
                {
                    lb_Lyrics.SelectedItem = selected;
                    var display = lyrics.Lyrics.SkipWhile(o => o != selected).Skip(3).FirstOrDefault();
                    if (display != null)
                        lb_Lyrics.ScrollIntoView(display);
                    else
                        lb_Lyrics.ScrollIntoView(selected);
                }
            }
        }

        #endregion

        #region LyricModel
        private ILrcFile _lyricModel;

        public ILrcFile LyricModel
        {
            get; set;
        }
        #endregion

        #region UserControl Handler

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string ImgPath = CurrentTrack?.PictureUrl;
            var action = BlurCanvas.RunOnGameLoopThreadAsync(async () =>
            {
                await ReflashBackground(BlurCanvas, ImgPath);
                BlurCanvas.Invalidate();

            });
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region BlurCanvas Event Handler

        private void BlurCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (scaleEffect == null) return;
            args.DrawingSession.DrawImage(scaleEffect);
            sender.Paused = true;
        }

        private void BlurCanvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {

        }

        private void BlurCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(ReflashBackground(sender, CurrentTrack?.PictureUrl).AsAsyncAction());
        }

        #endregion

        #region BlurCanvas Helper Methods

   
        private async Task ReflashBackground(CanvasAnimatedControl resource, string imgPath)
        {
            if (imgPath == null) return;
            if (imgbackground == null || oldImgPath != imgPath)
            {
                oldImgPath = imgPath;
                imgbackground = await CanvasBitmap.LoadAsync(resource, new Uri(imgPath), 96);
            }
            if (blurEffect == null)
            {
                blurEffect = new GaussianBlurEffect()
                {
                    BlurAmount = 40.0f,
                    BorderMode = EffectBorderMode.Soft,
                    Optimization = EffectOptimization.Speed
                };
            }
            blurEffect.Source = imgbackground;
            if (scaleEffect == null)
            {
                scaleEffect = new ScaleEffect()
                {
                    CenterPoint = new System.Numerics.Vector2()
                };
            }
            scaleEffect.Source = blurEffect;
            scaleEffect.Scale = new System.Numerics.Vector2(await ComputeScaleFactor());
            resource.Paused = false;
        }
        private async Task<float> ComputeScaleFactor()
        {
            float scaleFactor = 1;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                       () =>
                       {
                           if (imgbackground == null || ActualWidth == 0 || ActualHeight == 0)
                           {
                               scaleFactor = 1;
                               return;
                           }
                           float screenRatio = (float)ActualWidth / (float)ActualHeight;
                           float imgRatio = imgbackground.SizeInPixels.Width / (float)imgbackground.SizeInPixels.Height;
                           if (imgRatio < screenRatio)
                           {
                               scaleFactor = (float)(ActualWidth / imgbackground.SizeInPixels.Width);
                           }
                           else
                           {
                               scaleFactor = (float)(ActualHeight / imgbackground.SizeInPixels.Height);
                           }
                       });
            return scaleFactor;



        }
        #endregion

        #region Messenget Helper Methods
        private void RegisterMessenger()
        {

        }

        private void UnRegisterMessenger()
        {
            Messenger.Default.Unregister(this);
        }

        private void HandleMyMusicViewMsg(NotificationMessage<SongList> msg)
        {
            if (msg.Notification == "GotoSongListView")
            {
            }
        }

      

        #endregion
        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
            //Messenger.Default.Send(new NotificationMessage("HidePlayingView"), Token);
        }

        private void IsPlayingChanged(object sender, bool e)
        {
            OnIsOpenChanged(IsOpen);
        }
    }
}
