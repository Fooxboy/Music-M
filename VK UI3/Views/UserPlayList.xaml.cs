using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using VK_UI3.Controls;
using VK_UI3.DB;
using VK_UI3.Helpers;
using VK_UI3.Views.ModalsPages;
using VK_UI3.VKs;
using VkNet.Model.Attachments;
using VkNet.Utils;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VK_UI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  enum OpenedPlayList
    {
        all,
        UserPlayList,
        UserAlbums
    }
    public class UserPlayListParameters {
        public VkCollection<AudioPlaylist> VKaudioPlaylists;
        public uint offset = 0;
        public long? UserId = null;
        public OpenedPlayList openedPlayList;
        public bool LoadedAll;
    }
    public sealed partial class UserPlayList : Microsoft.UI.Xaml.Controls.Page
    {

        public ObservableRangeCollection<AudioPlaylist> audioPlaylists { get; set; } = new ObservableRangeCollection<AudioPlaylist>();

        public UserPlayList()
        {
            this.InitializeComponent();
            this.Loaded += UserPlayList_Loaded;
            this.Loading += UserPlayList_Loading;

            this.Unloaded += UserPlayList_Unloaded;
            
        }
        public UserPlayList(Audio audio)
        {
            this.InitializeComponent();
            this.Loaded += UserPlayList_Loaded;
            this.Loading += UserPlayList_Loading;
            this.Unloaded += UserPlayList_Unloaded;
            this.audio = audio;
        }

        private void UserPlayList_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= UserPlayList_Loaded;
            this.Loading -= UserPlayList_Loading;

            this.Unloaded -= UserPlayList_Unloaded;

            if (scrollViewer != null)
                this.scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
        }

    


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            this.Loaded -= UserPlayList_Loaded;
            this.Loading -= UserPlayList_Loading;
            try
            {
                if (scrollViewer != null)
                this.scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            }
            catch (Exception ex)
            { 
            
            }
        }

        Audio audio = null;

        UserPlayListParameters parameters;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var userPlayList = e.Parameter as UserPlayListParameters;
            if (userPlayList == null)
                return;
            parameters = userPlayList;

           

        }
     


        private bool CheckIfAllContentIsVisible(ScrollViewer scrollViewer)
        {
            if (scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight)
            {
                return true;
            }
            return false;
        }

        private void UserPlayList_Loading(FrameworkElement sender, object args)
        {
            if (parameters.LoadedAll)
            {
                LoadingIndicator.Visibility = Visibility.Collapsed;
            }
            if (parameters.openedPlayList != OpenedPlayList.UserPlayList)
            {
                CreateButton.Visibility = Visibility.Collapsed;
            }
            if (audio != null)
            {
                this.Background = Application.Current.Resources["AcrylicBackgroundFillColorDefaultBrush"] as SolidColorBrush;
                CreateButton.Visibility = Visibility.Collapsed;
                this.Margin = new Thickness(-25);
                Cancel.Visibility = Visibility.Visible;
            }
        }
        ScrollViewer scrollViewer { get; set; }
        private void UserPlayList_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = SmallHelpers.FindScrollViewer(gridV);
            if (audio != null)
            {
                gridV.SelectionMode = ListViewSelectionMode.Single;
                this.parameters.openedPlayList = OpenedPlayList.UserPlayList;
                MainGrid.Background = (Brush)Application.Current.Resources["AcrylicBackgroundFillColorDefaultBrush"];
            }

            if (parameters.VKaudioPlaylists != null)
            {
                addToList(parameters.VKaudioPlaylists);
            }
            else
            { 
                
            }

            if (CheckIfAllContentIsVisible(scrollViewer))
            {
                loadMoreAsync();
            }

            this.scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

            var isAtBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 50;
            if (isAtBottom)
            {
                loadMoreAsync();
            }
        }

        void addToList(VkCollection<AudioPlaylist> VKaudioPlaylists)
        {
            audioPlaylists.AddRange(VKaudioPlaylists.Where(item =>
            {
                bool isUserPlaylist = item.OwnerId == AccountsDB.activeAccount.id && item.Original == null;
                return parameters.openedPlayList switch
                {
                    OpenedPlayList.all => true,
                    OpenedPlayList.UserPlayList => isUserPlaylist,
                    OpenedPlayList.UserAlbums => !isUserPlaylist,
                    _ => false
                };
            }));
        }


      
       

         internal WeakEventManager selectedPlayList = new WeakEventManager();
        private async Task loadMoreAsync()
        {
            if (parameters.UserId == null)
                parameters.UserId = AccountsDB.activeAccount.id;
            if (parameters.LoadedAll) return;
            LoadingIndicator.IsActive = true;

            var a = await VK.api.Audio.GetPlaylistsAsync((long)parameters.UserId, 100, parameters.offset);

            addToList(a);
            LoadingIndicator.IsActive = false;
            if (a.Count != 100)
            {
                parameters.LoadedAll = true;
                LoadingIndicator.Visibility = Visibility.Collapsed;
            }
    

            if (CheckIfAllContentIsVisible(scrollViewer))
            {
                loadMoreAsync();
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
    

            var a = new CreatePlayList();
            dialog.Content = a;
            dialog.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            a.cancelPressed.AddHandler((s, e) =>
            {
                dialog.Hide();
                dialog = null;

                if (s != null && s is AudioPlaylist)
                { 
                    var playlist = s as AudioPlaylist;
                    this.audioPlaylists.Insert(0, playlist);
                }
            });

            dialog.ShowAsync();
        }

        private void gridV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // �������� �� ������� ��������� ���������
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            // ��������� ������� ���������� ��������
            int selectedIndex = gridV.Items.IndexOf(e.AddedItems[0]);

            // �������� �� ������������ �������
            if (selectedIndex < 0 || selectedIndex >= audioPlaylists.Count)
            {
                return;
            }

            // ��������� ID ���������
            var playlistId = audioPlaylists[selectedIndex].Id;

            // ���������� ����� � ��������
            VK.api.Audio.AddToPlaylistAsync((long)audioPlaylists[selectedIndex].OwnerId, playlistId, new List<string> { $"{audio.OwnerId}_{audio.Id}" });

            // ����� �������
            selectedPlayList?.RaiseEvent(playlistId, EventArgs.Empty);
        }

        private void gridV_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (audio == null) return;
            var control = args.ItemContainer.ContentTemplateRoot as PlaylistControl;
            if (control != null)
            {
                control.IsHitTestVisible = false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            selectedPlayList?.RaiseEvent(null, EventArgs.Empty);
        }
    }
}
