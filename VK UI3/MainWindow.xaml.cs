﻿#define SKIP_ERROR_PRONE_CODE
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using SetupLib;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VK_UI3.DB;
using VK_UI3.Views;
using VK_UI3.Views.LoginWindow;
using Windows.ApplicationModel;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using System.IO;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Windows.UI.ViewManagement;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;
using vkPosterBot.DB;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Microsoft.UI.Input;
using Windows.Foundation;
using Microsoft.AppCenter.Utils;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VK_UI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Microsoft.UI.Xaml.Window
    {
        internal static HWND hvn;
        AppWindow m_AppWindow = null;
       public static  MainWindow mainWindow;
        public MainWindow()
        {
            
            this.InitializeComponent();

            m_AppWindow = this.AppWindow;

            mainWindow = this;

            contentFrame = ContentFrame;
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);
            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;



            AppWindowTitleBar m_TitleBar = m_AppWindow.TitleBar;



            CheckMica();
            updateMica += OnUpdateMica;



            m_TitleBar.ButtonForegroundColor = (Application.Current.RequestedTheme == ApplicationTheme.Dark) 
                ? Colors.White : Colors.Black;
            UISettings uI = new();
            uI.ColorValuesChanged += UI_ColorValuesChanged; ;


            



            var navigationInfo = new NavigationInfo { SourcePageType = this };

             if (AccountsDB.GetAllAccounts().Count == 0) {

                GoLogin();
             }else
                ContentFrame.Navigate(typeof(MainView), navigationInfo, new DrillInNavigationTransitionInfo());
       

            dispatcherQueue = this.DispatcherQueue;

             SubClassing();

            string assetsPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            
            // Составить путь к иконке
            string iconPath = Path.Combine(assetsPath, "icon.ico");
            LoadIcon(iconPath);



            hvn = (HWND)WinRT.Interop.WindowNative.GetWindowHandle(this);

            AnimatedButtonScaleStoryboardShow.Completed += AnimatedButtonScaleStoryboardShow_Completed;
            AnimatedButtonScaleStoryboardDisShow.Completed += AnimatedButtonScaleStoryboardShow_Completed;

            DownLoadBTNScaleShow.Completed += AnimatedButtonScaleStoryboardShow_Completed;
            DownLoadBTNScaleHide.Completed += AnimatedButtonScaleStoryboardShow_Completed;

            this.Activated += activated;
          
        }

        private void AnimatedButtonScaleStoryboardShow_Completed(object sender, object e)
        {
            SetRegionsForCustomTitleBar();
        }

        public void MainWindow_showRefresh()
        {
   
                AnimatedButtonScaleStoryboardDisShow.Pause();
                AnimatedButtonScaleStoryboardShow.Begin();

            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }

        }

        public void MainWindow_hideRefresh()
        {
            AnimatedButtonScaleStoryboardShow.Pause();
            AnimatedButtonScaleStoryboardDisShow.Begin();

            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }


        }

        public void MainWindow_showDownload()
        {
            DownLoadBTNScaleHide.Pause();
            DownLoadBTNScaleShow.Begin();


            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }

        }

        public void MainWindow_hideDownload()
        {
            DownLoadBTNScaleShow.Pause();
            DownLoadBTNScaleHide.Begin();

            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }
        }


        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }
        }

        private void SetRegionsForCustomTitleBar()
        {
            // Specify the interactive regions of the title bar.

            double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;
            var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(m_AppWindow.Id);


            // Получите все дочерние элементы StackPanel
            var children = AppTitleBar.Children;

            // Создайте список для хранения прямоугольников каждого элемента
            List<Windows.Graphics.RectInt32> rects = new List<Windows.Graphics.RectInt32>();

            foreach (var child in children)
            {
                var frameworkElement = child as FrameworkElement;
                // Пропустите TitIcon и AppTitle
                if (frameworkElement.Name == "TitIcon" || frameworkElement.Name == "AppTitle")
                    continue;

                // Получите границы каждого элемента
                var transform = child.TransformToVisual(null);
                var bounds = transform.TransformBounds(new Rect(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));

                // Преобразуйте координаты DPI
             
                var rect = new Windows.Graphics.RectInt32(
                    _X: (int)Math.Round(bounds.X * scaleAdjustment),
                    _Y: (int)Math.Round(bounds.Y * scaleAdjustment),
                    _Width: (int)Math.Round(bounds.Width * scaleAdjustment),
                    _Height: (int)Math.Round(bounds.Height * scaleAdjustment)
                );

                // Добавьте прямоугольник в список
                rects.Add(rect);
            }

            // Установите области, которые будут прозрачными для кликов мыши
            nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rects.ToArray());


        }



        private Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
        {
            return new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
        }

        private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // Set the initial interactive regions.
                SetRegionsForCustomTitleBar();
            }
        }

        private void activated(object sender, WindowActivatedEventArgs args)
        {

            checkUpdate();
        }

        private void OnUpdateMica(object sender, EventArgs e)
        {
            CheckMica();
        }

        public static EventHandler updateMica;

        private void CheckMica()
        {
            var set = SettingsTable.GetSetting("backDrop");
            if (set != null)
            {
                if (!(SystemBackdrop is MicaBackdrop))
                SystemBackdrop = new MicaBackdrop()
                { Kind = MicaKind.BaseAlt };
            }
            else
            {
                if (!(SystemBackdrop is DesktopAcrylicBackdrop))
                    SystemBackdrop = new DesktopAcrylicBackdrop()
                    {  };
            }
        }

        private void UI_ColorValuesChanged(UISettings sender, object args)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.High,
           () =>
           {
               AppWindow m_AppWindow = this.AppWindow;
               AppWindowTitleBar m_TitleBar = m_AppWindow.TitleBar;

               m_TitleBar.ButtonForegroundColor = (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                   ? Colors.White : Colors.Black;
           });
        }

        public static DispatcherQueue dispatcherQueue { get; private set; }

        public static Frame contentFrame;

        public void GoLogin()
        {

            ContentFrame.Navigate(typeof(Login), this, new DrillInNavigationTransitionInfo());
        }


        Microsoft.Win32.SafeHandles.SafeFileHandle iIcon;
        private void LoadIcon(string aIconName)
        {
            // Resolve icon path
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, aIconName);
            // Get window handle
            var hwnd = new Windows.Win32.Foundation.HWND(WinRT.Interop.WindowNative.GetWindowHandle(this));
            // Load our image
            iIcon = PInvoke.LoadImage(null, iconPath, GDI_IMAGE_TYPE.IMAGE_ICON, 16, 16, IMAGE_FLAGS.LR_LOADFROMFILE);
            //Set our icon
            PInvoke.SendMessage(hwnd, PInvoke.WM_SETICON, new WPARAM(0), new LPARAM(iIcon.DangerousGetHandle()));
        }



        private void checkUpdate()
        {
            

            Task.Run(async () =>
            {

                Windows.ApplicationModel.Package package = Windows.ApplicationModel.Package.Current;
                PackageId packageId = package.Id;
                var version = packageId.Version;
                var currentVersion = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

                var appUpdater = new AppUpdater(currentVersion);




                var updateAvailable = await appUpdater.CheckForUpdates();

                if (updateAvailable)
                {
                 
                    // Создайте новое окно
                    dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () => {
                        ContentFrame.Navigate(typeof(UpdatePage), appUpdater, new DrillInNavigationTransitionInfo());
                    });
                    
                }
            });
      
        }



        private delegate IntPtr WinProc(HWND hWnd, uint Msg, WPARAM wParam, IntPtr lParam);
        private WinProc newWndProc = null;
        private IntPtr oldWndProc = IntPtr.Zero;

      
        private void SubClassing()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            if (hwnd == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Failed to get window handler: error code {error}");
            }

            newWndProc = new(NewWindowProc);


            oldWndProc = SetWindowLong((HWND)hwnd, WINDOW_LONG_PTR_INDEX.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc)); 
            
            if (oldWndProc == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Failed to set GWL_WNDPROC: error code {error}");
            }
        }



        const uint WM_GETMINMAXINFO = 0x0024;
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        private static IntPtr SetWindowLong(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex, IntPtr newProc)
        {
            if (Environment.Is64BitOperatingSystem)
            {
#if X64
                return PInvoke.SetWindowLongPtr(hWnd, nIndex, newProc);
#endif
            }

            return new IntPtr(PInvoke.SetWindowLong(hWnd, nIndex, newProc.ToInt32()));

        }






        private IntPtr NewWindowProc(HWND hWnd, uint Msg, WPARAM wParam, IntPtr lParam)
        {
            switch (Msg)
            {
                case WM_GETMINMAXINFO:
                    var dpi = PInvoke.GetDpiForWindow(hWnd);
                    float scalingFactor = (float)dpi / 96;

                    MINMAXINFO minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
                    minMaxInfo.ptMinTrackSize.x = (int)(500 * scalingFactor);
                    minMaxInfo.ptMinTrackSize.y = (int)(500 * scalingFactor);
                    Marshal.StructureToPtr(minMaxInfo, lParam, true);
                    break;
            }
            WNDPROC oldWndProcDelegate = (WNDPROC)Marshal.GetDelegateForFunctionPointer(oldWndProc, typeof(WNDPROC));
            return PInvoke.CallWindowProc(oldWndProcDelegate, hWnd, Msg, wParam, lParam);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        //    AnimatedIcon.SetState(this.SearchAnimatedIcon, "PointerOver");

        }
        public static event EventHandler? onRefreshClicked;
        public static event EventHandler? onDownloadClicked;
        public static void onRefreshClickedvoid()
        {
            MainWindow.mainWindow.RotationStoryboard.Begin();
            onRefreshClicked?.Invoke(null, EventArgs.Empty);

        }

        public static void onRefreshClicked_clear()
        {
            onRefreshClicked = null;

        }
        private void RefreshClick_Click(object sender, RoutedEventArgs e)
        {
            RotationStoryboard.Begin();
            onRefreshClicked?.Invoke(this, EventArgs.Empty);

        }

        private void DownLoadBTN_Click(object sender, RoutedEventArgs e)
        {
            ScaleStoryboard.Begin();
            onDownloadClicked?.Invoke(this, EventArgs.Empty);
        }
    }

}
