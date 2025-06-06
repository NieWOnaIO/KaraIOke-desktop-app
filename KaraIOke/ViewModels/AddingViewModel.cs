using System.Diagnostics;
using System.Windows.Input;
using KaraIOke.Models;
using KaraIOke.Services.AppEnvironment;

namespace KaraIOke.ViewModels;

public class AddingViewModel : AbstractPlaylistViewModel
{
    public ICommand AddSongToPlaylist { private set; get; }

    public Song Song { get; set; }

    public AddingViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        AddSongToPlaylist = new Command(
            execute: (object playlistName) =>
            {
                // if ((string)playlistName == "Historia" || (string)playlistName == "Dodaj Playlistę")
                //     return;

                var appEnv = serviceProvider.GetService<AppEnvironmentService>();
                appEnv.PlaylistService.AddSong((string)playlistName, Song);
                appEnv.DownloadService.SaveSong(Song);
            }
        );
    }
}