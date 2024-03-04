using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MusicX.Core.Models;
using System;
using System.Collections.ObjectModel;
using VK_UI3.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VK_UI3.Controls.Blocks
{
    public sealed partial class ListPlaylists : UserControl
    {
        public ListPlaylists()
        {
            this.InitializeComponent();

            DataContextChanged += RecommsPlaylistBlock_DataContextChanged;
            this.Loading += ListPlaylists_Loading;
        }

        private void ListPlaylists_Loading(FrameworkElement sender, object args)
        {
            try
            {
                if (DataContext is not Block block)
                    return;

                var pl = (DataContext as Block).Playlists;


                foreach (var item in pl)
                {
                    playlists.Add(item);
                }
            }
            catch (Exception ex)
            {
                AppCenterHelper.SendCrash(ex);


            }
        }

        ObservableCollection<Playlist> playlists = new();

        private void RecommsPlaylistBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            
        }
    }
}
