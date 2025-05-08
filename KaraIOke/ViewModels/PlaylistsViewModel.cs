using System.Windows.Input;

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
                    await _navigationService.PushPlaylist(playlistName);
                }
            }
        );

        DeletePlaylist = new Command<string>(
            execute: async (playlistName) =>
            {
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