using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VK_UI3.Controllers;
using VK_UI3.DB;
using VK_UI3.Helpers;
using VK_UI3.Helpers.Animations;
using VK_UI3.Views.ModalsPages;
using VK_UI3.VKs;
using VK_UI3.VKs.IVK;
using VkNet.Model.Attachments;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace VK_UI3.Views
{
    public record PlaylistData(long PlaylistId, long OwnerId, string AccessKey);
    public sealed partial class PlayListPage : Microsoft.UI.Xaml.Controls.Page
    {
        public PlayListPage()
        {
            this.InitializeComponent();
            this.Loaded += PlayListPage_Loaded;
            MainText = new AnimationsChangeText(textBlock1, this.DispatcherQueue);
            descriptionText = new AnimationsChangeText(DescriptionText, this.DispatcherQueue);
        }

       

        public IVKGetAudio vkGetAudio = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            vkGetAudio = e.Parameter as IVKGetAudio;
            if (vkGetAudio == null)
            {
                vkGetAudio = new UserAudio(AccountsDB.activeAccount.id, this.DispatcherQueue);
            }




            vkGetAudio.onPhotoUpdated += VkGetAudio_onPhotoUpdated;
            vkGetAudio.onListUpdate += VkGetAudio_onListUpdate; ; ;

            vkGetAudio.onNameUpdated += VkGetAudio_onNameUpdated;
            vkGetAudio.onCountUpDated += VkGetAudio_onCountUpDated;
            vkGetAudio.onInfoUpdated += VkGetAudio_onInfoUpdated;

            updateUI(true);


            base.OnNavigatedTo(e);
        }
        AnimationsChangeText MainText;
        AnimationsChangeText descriptionText;
        private void updateUI(bool load = false)
        {
            this.DispatcherQueue.TryEnqueue(async () =>
            {

                SmallHelpers.AddImagesToGrid(GridThumbs, vkGetAudio.getPhotosList(), DispatcherQueue);
                if (vkGetAudio is (PlayListVK))
                {
                    var playlist = (vkGetAudio as PlayListVK).playlist;
                    if (!playlist.Permissions.Edit)
                    {
                        stackPanel.Items.Remove(EditPlaylist);
                    }
                    
                    if (playlist.Permissions.Edit && !playlist.Permissions.Follow)
                        stackPanel.Items.Remove(AddPlaylist);
                    
                    
                    else
                    if (playlist.IsFollowing && !playlist.Permissions.Edit)
                    {
                        iconAdd.Symbol = Symbol.Delete;
                        textAdd.Text = "�������";
                    }

                    if (load)
                        DescriptionText.Text = playlist.Description;
                    else
                        descriptionText.ChangeTextWithAnimation(playlist.Description);

                    FollowersText.Visibility = Visibility.Visible;
                    FollowersText.Text = $"{playlist.Followers} �����������";


                    string GenresText = string.Join(", ", playlist.Genres.Select(g => g.Name));


                    Genres.Text = $"�����: {GenresText}" ;
                    Genres.Visibility = Visibility.Visible;


                    string dateInDesiredFormat = playlist.CreateTime.ToString("d MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    string updateTimeF = playlist.UpdateTime.ToString("d MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);

                    if (playlist.UpdateTime != DateTime.MinValue && playlist.CreateTime.Date != playlist.UpdateTime.Date)
                    {
                        Year.Text = $"������: {dateInDesiredFormat}, ������� {updateTimeF}";
                    }
                    else
                    {
                        Year.Text = $"������: {dateInDesiredFormat}";
                    }
                    Year.Visibility = Visibility.Visible;

                    Plays.Text = $"������������� : {playlist.Plays}";
                    Plays.Visibility = Visibility.Visible;



                }
                else
                {
                    stackPanel.Items.Remove(AddPlaylist);
                    stackPanel.Items.Remove(EditPlaylist);

                   
                    MainText.ChangeTextWithAnimation(vkGetAudio.name);
                }
                MainText.ChangeTextWithAnimation(vkGetAudio.name);

            });

        }

        private void VkGetAudio_onInfoUpdated(object sender, EventArgs e)
        {
            updateUI();
        }

        private void VkGetAudio_onCountUpDated(object sender, EventArgs e)
        {
            updateUI();
        }

        private void VkGetAudio_onNameUpdated(object sender, EventArgs e)
        {
            updateUI();
        }

        private void VkGetAudio_onListUpdate(object sender, EventArgs e)
        {
            blockLoad = false;
            if (vkGetAudio.itsAll)
                LoadingIndicator.Visibility = Visibility.Collapsed;
        }

    
        private void VkGetAudio_onPhotoUpdated(object sender, EventArgs e)
        {
            SmallHelpers.AddImagesToGrid(GridThumbs, vkGetAudio.getPhotosList(), DispatcherQueue);
        }

      
        private ScrollViewer FindScrollViewer(DependencyObject d)
        {
            if (d is ScrollViewer)
                return d as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var sw = FindScrollViewer(VisualTreeHelper.GetChild(d, i));
                if (sw != null) return sw;
            }

            return null;
        }
        ScrollViewer scrollViewer;
        private void PlayListPage_Loaded(object sender, RoutedEventArgs e)
        {
            // ������� ScrollViewer ������ ListView

            scrollViewer = FindScrollViewer(TrackListView);
            if (scrollViewer != null)
            {
                // ������������� �� ������� ��������� ���������
                scrollViewer.ViewChanged += ScrollViewer_ViewChanged; ;
            }
        }

        bool blockLoad = false;
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            
            var isAtBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;
            if (isAtBottom)
            {
               
                if (vkGetAudio.itsAll)
                {
                    LoadingIndicator.Visibility = Visibility.Collapsed;
                    return;
                }
                if (blockLoad) return;
                blockLoad = true;
                LoadingIndicator.Visibility = Visibility.Visible;
                vkGetAudio.GetTracks();
                LoadingIndicator.IsActive = true;
            }
        }

        private async void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var playlist = (vkGetAudio as PlayListVK).playlist;

            _ = Task.Run(
                     async () =>
                     {

                         if (playlist.IsFollowing)
                         {
                             await VK.vkService.DeletePlaylistAsync(playlist.Id, playlist.OwnerId);
                             playlist.IsFollowing = false;
                         }
                         else
                         {
                             await VK.vkService.AddPlaylistAsync(playlist.Id, playlist.OwnerId, playlist.AccessKey);
                             playlist.IsFollowing = true;


                         }
                         updateUI();


                     });

        }

        private void PlayPlaylist_Click(object sender, RoutedEventArgs e)
        {

            vkGetAudio.currentTrack = 0;
            AudioPlayer.PlayList(vkGetAudio);
        }

        private void EditPlaylist_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;


            var a = new CreatePlayList((vkGetAudio as PlayListVK).playlist);

            dialog.Content = a;
            dialog.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            a.cancelPressed += (s, e) =>
            {
                if (s != null && s is AudioPlaylist)
                {
                    (vkGetAudio as PlayListVK).playlist = (AudioPlaylist)s;
                    updateUI();
                    vkGetAudio.name = (s as AudioPlaylist).Title;
                }
                try
                {
                    dialog.Hide();
                    dialog = null;
                }
                catch
                {

                }
            };

            dialog.ShowAsync();

        }

        private void DownloadPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
