using KaraIOke.Services.AppEnvironment;
using KaraIOke.Views;
using KaraIOke.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace KaraIOke.Services.Navigation;

public class NavigationService
{
    INavigation? _navigation;
    readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [MemberNotNull(nameof(_navigation))]
    void initData()
    {
        if (_navigation is null)
        {
            var mainView = _serviceProvider.GetService<MainView>() ?? throw new InvalidOperationException("MainView is not registered");
            _navigation = mainView.Navigation;
        }
    }

    public async Task PushSearchSong()
    {
        initData();

        var searchViewModel = _serviceProvider.GetService<SearchViewModel>() ?? throw new InvalidOperationException("SearchViewModel is not registered");
        searchViewModel.loadData();
        if (_navigation.NavigationStack.Last() is SearchView)
        {
            return;
        }

        var mainViewModel = _serviceProvider.GetService<MainViewModel>() ?? throw new InvalidOperationException("MainViewModel is not registered");
        searchViewModel.SongName = mainViewModel.SongName;

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
        initData();

        await _navigation.PopAsync();
    }

    public async Task PushPlaylist(string playlistName)
    {
        initData();

        var playlistDetailViewModel = _serviceProvider.GetService<PlaylistDetailsViewModel>() ?? throw new InvalidOperationException("PlaylistDetailsViewModel is not registered");
        playlistDetailViewModel.loadData(playlistName);

        var playlistView = _serviceProvider.GetService<PlaylistDetailsView>();
        await _navigation.PushAsync(playlistView);
    }

    public async Task PushPlaylistList()
    {
        initData();

        var playlistsViewModel = _serviceProvider.GetService<PlaylistsViewModel>() ?? throw new InvalidOperationException("PlaylistsViewModel is not registered");
        playlistsViewModel.loadData();

        var playlistListView = _serviceProvider.GetService<PlaylistsView>();
        await _navigation.PushAsync(playlistListView);
    }

    public async Task PushAdding()
    {
        initData();

        var addingViewModel = _serviceProvider.GetService<AddingViewModel>() ?? throw new InvalidOperationException("AddingViewModel is not recognized");
        addingViewModel.loadData();

        var addingView = _serviceProvider.GetService<AddingView>();
        await _navigation.PushAsync(addingView);
    }
}