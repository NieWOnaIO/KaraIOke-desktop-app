using System.Windows.Input;
using KaraIOke.Models;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Download;
using KaraIOke.Services.Navigation;

namespace KaraIOke.ViewModels;

public partial class PlayerViewModel
{
    protected readonly NavigationService _navigationService;
    protected readonly IDownloadService _downloadService;

    private Song _song;
    private bool _readyToPlay = false;

    public async Task SetSong(Song song)
    {
        _song = song;
        // Task.Run(async () =>
        // {
            await _downloadService.QueryDownload(song);
            _readyToPlay = true;
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
    }

    public ICommand GoBack { private set; get; }
}