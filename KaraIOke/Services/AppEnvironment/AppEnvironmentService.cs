using KaraIOke.Services.Playlists;
using KaraIOke.Services.Search;
using KaraIOke.Views;

namespace KaraIOke.Services.AppEnvironment;

public class AppEnvironmentService
{
    private readonly ISearchService _searchService;
    private readonly ISearchService _searchMockService;

    public AppEnvironmentService(ISearchService searchMockService, ISearchService searchService)
    {
        _searchService = searchService;
        _searchMockService = searchMockService;
    }

    public ISearchService SearchService { get; private set; }
    public IPlaylistService PlaylistService { get; private set; }

    public void updateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            SearchService = _searchMockService;
            PlaylistService = new PlaylistMockService();
        } 
        else 
        {
            SearchService = _searchService;
            //TODO: implement PlaylistService
            // PlaylistService = ...
        }
    }
}