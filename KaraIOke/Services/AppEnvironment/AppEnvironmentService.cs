using System.Diagnostics.CodeAnalysis;
using KaraIOke.Services.Playlists;
using KaraIOke.Services.Search;

namespace KaraIOke.Services.AppEnvironment;

public class AppEnvironmentService
{
    private readonly ISearchService _searchService;
    private readonly ISearchService _searchMockService;

    private readonly IPlaylistService _playlistService;
    private readonly IPlaylistService _playlistMockService;

    public AppEnvironmentService(ISearchService searchMockService, ISearchService searchService, IPlaylistService playlistMockService, IPlaylistService playlistService)
    {
        _searchService = searchService;
        _searchMockService = searchMockService;

        _playlistService = playlistService;
        _playlistMockService = playlistMockService;

        updateDependencies(false);
    }

    public ISearchService SearchService { get; private set; }
    public IPlaylistService PlaylistService { get; private set; }

    [MemberNotNull(nameof(SearchService), nameof(PlaylistService))]
    public void updateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            SearchService = _searchMockService;
            PlaylistService = _playlistMockService;
        }
        else
        {
            SearchService = _searchService;
            PlaylistService = _playlistService;
        }
    }
}