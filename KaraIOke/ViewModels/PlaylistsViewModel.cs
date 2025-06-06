using System.Windows.Input;
using KaraIOke.Services.Playlists;

namespace KaraIOke.ViewModels;

public class PlaylistsViewModel : AbstractPlaylistViewModel
{
    public ICommand GoToPlaylist { private set; get; }

    public ICommand DeletePlaylist { private set; get; }

    public PlaylistsViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        GoToPlaylist = new Command<string>(
            execute: async (playlistName) =>
            {
                if (!string.IsNullOrEmpty(playlistName))
                {
                    if (playlistName == "Dodaj Playlistę")
                    {
                        var count = _appEnvironmentService.PlaylistService.GetAllPlaylistsNames().Count;
                        _appEnvironmentService.PlaylistService.AddPlaylist(new Models.Playlist($"Playlista {count}", []));

                        loadData();
                    }
                    else
                    {
                        await _navigationService.PushPlaylist(playlistName);
                    }
                }
            }
        );

        DeletePlaylist = new Command<string>(
            execute: async (playlistName) =>
            {
                if (playlistName == "Historia" || playlistName == "Dodaj Playlistę")
                {
                    return;
                }
                if (!string.IsNullOrEmpty(playlistName))
                {
                    await Task.Run(() =>
                    {
                        _appEnvironmentService.PlaylistService.DeletePlaylist(playlistName);
                        loadData();
                    });
                }
            }
        );
    }
}