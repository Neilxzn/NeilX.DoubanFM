using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace NeilX.DoubanFM.Controls
{
    public delegate void FlyoutCloseRequested(object sender, EventArgs e);
    public delegate void RightSidebarNavigated(object sender, EventArgs p);
    public delegate void RightSidebarClosed(object sender, EventArgs e);
    public delegate void ContentSizeChanged(double newWidth);

    [TemplatePart(Name = ContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = RightFlyoutContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = RightFlyoutFadeInName, Type = typeof(Storyboard))]
    [TemplatePart(Name = RightFlyoutFadeOutName, Type = typeof(Storyboard))]
    [TemplatePart(Name = RightFlyoutPlaneProjectionName, Type = typeof(PlaneProjection))]
    [TemplatePart(Name = RightFlyoutGridContainerName, Type = typeof(Grid))]
    [TemplatePart(Name = FlyoutBackgroundRectangleName, Type = typeof(Rectangle))]
    [TemplatePart(Name = FooterContentPresenterName, Type = typeof(ContentPresenter))]
    public sealed class SplitShell : Control
    {
        public event FlyoutCloseRequested FlyoutCloseRequested;
        public event RightSidebarNavigated RightSidebarNavigated;
        public event RightSidebarClosed RightSidebarClosed;
        public event ContentSizeChanged ContentSizeChanged;
        public TaskCompletionSource<bool> TemplateApplied = new TaskCompletionSource<bool>();

        private DispatcherTimer _windowResizerTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };
        private const string HambegerGridName = "HambegerGrid";
        private const string LeftContentPresenterName = "LeftPaneContentPresenter";
        private const string ContentPresenterName = "ContentPresenter";
        private const string RightFlyoutContentPresenterName = "RightFlyoutContentPresenter";
        private const string RightFlyoutFadeInName = "RightFlyoutFadeIn";
        private const string RightFlyoutFadeOutName = "RightFlyoutFadeOut";

        private const string CenterFlyoutPlanePresenterName = "CenterFlyoutPlanePresenter";
        private const string CenterFlyoutFadeInName = "CenterFlyoutFadeIn";
        private const string CenterFlyoutFadeOutName = "CenterFlyoutFadeOut";

        private const string RightFlyoutPlaneProjectionName = "RightFlyoutPlaneProjection";
        private const string RightFlyoutGridContainerName = "RightFlyoutGridContainer";
        private const string FlyoutBackgroundRectangleName = "FlyoutBackgroundRectangle";
        private const string FooterContentPresenterName = "FooterContentPresenter";

        private Grid _hambegerGrid;
        private Grid _rightFlyoutGridContainer;
        private Rectangle _flyoutBackgroundRectangle;
        private ContentPresenter _leftContentPresenter;
        private ContentPresenter _contentPresenter;
        private ContentPresenter _rightFlyoutContentPresenter;
        private ContentPresenter _footerContentPresenter;
        private ContentPresenter _centerFlyoutPlanePresenter;

        private PlaneProjection _rightFlyoutPlaneProjection;
        private Storyboard _rightFlyoutFadeIn;
        private Storyboard _rightFlyoutFadeOut;

        private Storyboard _centerFlyoutFadeIn;
        private Storyboard _centerFlyoutFadeOut;

        private bool _isRightFlyoutOpen;
        private bool _isCenterFlyoutOpen;

        public async void SetLeftPaneContentPresenter(object contentPresenter)
        {
            await TemplateApplied.Task;
            _leftContentPresenter.Content = contentPresenter;
        }
        public async void SetContentPresenter(object contentPresenter)
        {
            await TemplateApplied.Task;
            _contentPresenter.Content = contentPresenter;
        }

        public async void SetRightPaneContentPresenter(object content)
        {
            await TemplateApplied.Task;
            _rightFlyoutContentPresenter.Content = content;
            ShowRightFlyout();
        }

        public async void SetFooterContentPresenter(object content)
        {
            await TemplateApplied.Task;
            _footerContentPresenter.Content = content;
        }
        public async void SetFooterVisibility(object visibility)
        {
            await TemplateApplied.Task;
            _footerContentPresenter.Visibility = (Visibility)visibility;
        }
        public async void SetCenterFlyoutContent(object content)
        {
            await TemplateApplied.Task;
            _centerFlyoutPlanePresenter.Content = content;
            ShowCenterFlyout();
        }

        #region LeftPaneContent


        public bool IsLeftPaneContentOpen
        {
            get { return (bool)GetValue(IsLeftPaneContentOpenProperty); }
            set { SetValue(IsLeftPaneContentOpenProperty, value); }
        }

        public static readonly DependencyProperty IsLeftPaneContentOpenProperty =
            DependencyProperty.Register("IsLeftPaneContentOpen", typeof(bool), typeof(SplitShell), new PropertyMetadata(default(bool)));



        public DependencyObject LeftPaneContent
        {
            get { return (DependencyObject)GetValue(LeftPaneContentProperty); }
            set { SetValue(LeftPaneContentProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneContentProperty =
            DependencyProperty.Register("LeftPaneContent", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), LeftPaneContentPropertyChangedCallback));

        private static void LeftPaneContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetLeftPaneContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region Content Property
        public DependencyObject Content
        {
            get { return (DependencyObject)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), ContentPropertyChangedCallback));


        private static void ContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region RightPaneContent Property

        public DependencyObject RightFlyoutContent
        {
            get { return (DependencyObject)GetValue(RightFlyoutContentProperty); }
            set { SetValue(RightFlyoutContentProperty, value); }
        }

        public static readonly DependencyProperty RightFlyoutContentProperty = DependencyProperty.Register(
            nameof(RightFlyoutContent), typeof(DependencyObject), typeof(SplitShell),
            new PropertyMetadata(default(DependencyObject), RightFlyoutContentPropertyChangedCallback));

        private static void RightFlyoutContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetRightPaneContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region FooterContent Property

        public Visibility FooterVisibility
        {
            get { return (Visibility)GetValue(FooterVisibilityProperty); }
            set { SetValue(FooterVisibilityProperty, value); }
        }

        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register(
            "FooterVisibility", typeof(Visibility), typeof(SplitShell), new PropertyMetadata(Visibility.Visible, FooterVisibilityPropertyChangedCallback));

        private static void FooterVisibilityPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetFooterVisibility(dependencyPropertyChangedEventArgs.NewValue);
        }

        public DependencyObject FooterContent
        {
            get { return (DependencyObject)GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(
            "FooterContent", typeof(DependencyObject), typeof(SplitShell),
            new PropertyMetadata(default(DependencyObject), FooterContentPropertyChangedCallback));

        private static void FooterContentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var that = (SplitShell)dependencyObject;
            that.SetFooterContentPresenter(dependencyPropertyChangedEventArgs.NewValue);
        }
        #endregion

        #region CenterFlyoutContent Property


        public DependencyObject CenterFlyoutContent
        {
            get { return (DependencyObject)GetValue(CenterFlyoutContentProperty); }
            set { SetValue(CenterFlyoutContentProperty, value); }
        }

        public static readonly DependencyProperty CenterFlyoutContentProperty =
            DependencyProperty.Register("CenterFlyoutContent", typeof(DependencyObject), typeof(SplitShell), new PropertyMetadata(default(DependencyObject), CenterFlyoutContentPropertyChangedCallback));

        private static void CenterFlyoutContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var that = (SplitShell)d;
            that.SetCenterFlyoutContent(e.NewValue);
        }


        #endregion
        public SplitShell()
        {
            DefaultStyleKey = typeof(SplitShell);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _hambegerGrid = (Grid)GetTemplateChild(HambegerGridName);
            _leftContentPresenter = (ContentPresenter)GetTemplateChild(LeftContentPresenterName);
            _contentPresenter = (ContentPresenter)GetTemplateChild(ContentPresenterName);
            _rightFlyoutContentPresenter = (ContentPresenter)GetTemplateChild(RightFlyoutContentPresenterName);
            _rightFlyoutFadeIn = (Storyboard)GetTemplateChild(RightFlyoutFadeInName);
            _rightFlyoutFadeOut = (Storyboard)GetTemplateChild(RightFlyoutFadeOutName);
            _rightFlyoutPlaneProjection = (PlaneProjection)GetTemplateChild(RightFlyoutPlaneProjectionName);
            _rightFlyoutGridContainer = (Grid)GetTemplateChild(RightFlyoutGridContainerName);
            _flyoutBackgroundRectangle = (Rectangle)GetTemplateChild(FlyoutBackgroundRectangleName);
            _footerContentPresenter = (ContentPresenter)GetTemplateChild(FooterContentPresenterName);
            _centerFlyoutPlanePresenter = (ContentPresenter)GetTemplateChild(CenterFlyoutPlanePresenterName);
            _centerFlyoutFadeIn = (Storyboard)GetTemplateChild(CenterFlyoutFadeInName);
            _centerFlyoutFadeOut = (Storyboard)GetTemplateChild(CenterFlyoutFadeOutName);


            Responsive();

            Window.Current.SizeChanged += Current_SizeChanged;
            // _contentPresenter.Width = Window.Current.Bounds.Width;

            TemplateApplied.SetResult(true);

            _hambegerGrid.PointerPressed += HambegerGridPressed;

            _rightFlyoutGridContainer.Visibility = Visibility.Collapsed;
            if (_flyoutBackgroundRectangle != null)
                _flyoutBackgroundRectangle.Tapped += RightFlyoutGridContainerOnTapped;

            _windowResizerTimer.Tick += _windowResizerTimer_Tick;

            _rightFlyoutFadeOut.Completed += _rightFlyoutFadeOut_Completed;
            _rightFlyoutFadeIn.Completed += _rightFlyoutFadeIn_Completed;

            _centerFlyoutFadeIn.Completed += _centerFlyoutFadeIn_Completed;
            _centerFlyoutFadeOut.Completed += _centerFlyoutFadeOut_Completed;
        }



      
        private void HambegerGridPressed(object sender, PointerRoutedEventArgs e)
        {
            IsLeftPaneContentOpen = !IsLeftPaneContentOpen;
        }

        private void _rightFlyoutFadeIn_Completed(object sender, object e)
        {
            RightSidebarNavigated?.Invoke(null, new EventArgs());
        }

        private void _rightFlyoutFadeOut_Completed(object sender, object e)
        {
            _rightFlyoutContentPresenter.Content = null;
        }

        private void _centerFlyoutFadeIn_Completed(object sender, object e)
        {

        }
        private void _centerFlyoutFadeOut_Completed(object sender, object e)
        {
            _centerFlyoutPlanePresenter.Content = null;
        }


        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Responsive();
        }

        void Responsive()
        {
            if (Window.Current.Bounds.Width < 650)
            {
                _rightFlyoutContentPresenter.Height = Window.Current.Bounds.Height;
                _rightFlyoutContentPresenter.Width = Window.Current.Bounds.Width;
            }
            else
            {
                _rightFlyoutContentPresenter.Width = 650;
                _rightFlyoutContentPresenter.Height = Window.Current.Bounds.Height < 800 * 0.9 ? Window.Current.Bounds.Height : Window.Current.Bounds.Height * 0.9;
            }
            _windowResizerTimer.Stop();
            _windowResizerTimer.Start();
        }

        private void _windowResizerTimer_Tick(object sender, object e)
        {
            //_contentPresenter.Width = Window.Current.Bounds.Width;
            _windowResizerTimer.Stop();
            ContentSizeChanged?.Invoke(_contentPresenter.Width);
        }

        private void RightFlyoutGridContainerOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            FlyoutCloseRequested?.Invoke(null, new EventArgs());
        }

        void ShowRightFlyout()
        {
            _rightFlyoutFadeIn.Begin();
            IsRightFlyoutOpen = true;
        }

        public void HideRightFlyout()
        {
            _rightFlyoutFadeOut.Begin();
            IsRightFlyoutOpen = false;
            RightSidebarClosed?.Invoke(null, new EventArgs());
        }


        void ShowCenterFlyout()
        {
            _centerFlyoutFadeIn.Begin();
            IsCenterFlyoutOpen = true;
        }

        public void HideCenterFlyout()
        {
            _centerFlyoutFadeOut.Begin();
            IsCenterFlyoutOpen = false;
        }


        public bool IsRightFlyoutOpen
        {
            get
            {
                return _isRightFlyoutOpen;
            }
            private set
            {
                _isRightFlyoutOpen = value;
                _flyoutBackgroundRectangle.IsHitTestVisible = !value;
            }
        }

        public bool IsCenterFlyoutOpen
        {
            get
            {
                return _isCenterFlyoutOpen;
            }
            private set
            {
                _isCenterFlyoutOpen = value;

                _flyoutBackgroundRectangle.IsHitTestVisible = value;
            }
        }

    }
}
