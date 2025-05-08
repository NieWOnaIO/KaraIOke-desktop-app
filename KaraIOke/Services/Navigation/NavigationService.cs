using KaraIOke.Services.AppEnvironment;
using KaraIOke.Views;
using KaraIOke.ViewModels;

namespace KaraIOke.Services.Navigation;

public class NavigationService
{
    INavigation? _navigation;
    readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    void initData()
    {
        if (_navigation is null)
        {
            _navigation = _serviceProvider.GetService<MainView>().Navigation;
        }
    }

    public async Task PushSearchSong()
    {
        initData();

        _serviceProvider.GetService<SearchViewModel>().loadData();

        var searchView = _serviceProvider.GetService<SearchView>();
        await _navigation.PushAsync(searchView);
    }

    public async Task PushPlayer()
    {
        initData();

        var playerView = _serviceProvider.GetService<PlayerView>();
        await _navigation.PushAsync(playerView);
    }

    public async Task PopPage()
    {
        await _navigation.PopAsync();
    }

    public async Task PushPlaylist(string playlistName)
    {
        initData();

        _serviceProvider.GetService<PlaylistViewModel>().loadData(playlistName);

        var playlistView = _serviceProvider.GetService<PlaylistView>();
        await _navigation.PushAsync(playlistView);
    }

    public async Task PushPlaylistList()
    {
        initData();

        _serviceProvider.GetService<PlaylistListViewModel>().loadData();

        var playlistListView = _serviceProvider.GetService<PlaylistListView>();
        await _navigation.PushAsync(playlistListView);
    }
}