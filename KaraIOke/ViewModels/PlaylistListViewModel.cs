using System.Windows.Input;

namespace KaraIOke.ViewModels;

public class PlaylistListViewModel : AbstractPlaylistViewModel
{
    public ICommand GoToPlaylist {private set; get; }


    public PlaylistListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
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
    }
}