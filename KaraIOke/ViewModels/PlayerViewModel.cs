using System.Windows.Input;
using KaraIOke.Models;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Download;
using KaraIOke.Services.Navigation;
using Plugin.Maui.Audio;

namespace KaraIOke.ViewModels;

public partial class PlayerViewModel
{
    protected readonly NavigationService _navigationService;
    protected readonly IDownloadService _downloadService;

    private readonly IAudioManager _audioManager;

    private Song _song;
    private bool _readyToPlay = false;

    public async Task SetSong(Song song)
    {
        _song = song;
        // Task.Run(async () =>
        // {
        await _downloadService.QueryDownload(song);
        _readyToPlay = true;

        var player = _audioManager.CreatePlayer(_downloadService.GetSongAudio(_song).NoVocals);

        player.Play();

        // });
    }

    public PlayerViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered");
        var environment = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered");
        _downloadService = environment.DownloadService;

        GoBack = new Command(
            execute: async () =>
            {
                await _navigationService.PopPage();
            }
        );

        _audioManager = serviceProvider.GetService<IAudioManager>() ?? throw new InvalidOperationException("AudioManager not registered");
    }

    public ICommand GoBack { private set; get; }
}