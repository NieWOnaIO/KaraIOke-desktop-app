using System.Windows.Input;

namespace KaraIOke.ViewModels;

public class PlaylistsViewModel : INotifyPropertyChanged
{
    public ICommand GoToPlaylist { private set; get; }

    public ICommand DeletePlaylist { private set; get; }

    public PlaylistsViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered.");
        _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered.");

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