using System.Diagnostics.CodeAnalysis;
using KaraIOke.Services.Download;
using KaraIOke.Services.Playlists;
using KaraIOke.Services.Search;

namespace KaraIOke.Services.AppEnvironment;

public class AppEnvironmentService
{
    private readonly ISearchService _searchService;
    private readonly ISearchService _searchMockService;

    private readonly IPlaylistService _playlistService;
    private readonly IPlaylistService _playlistMockService;

    private readonly IDownloadService _downloadService;
    private readonly IDownloadService _downloadMockService;

    public AppEnvironmentService(ISearchService searchMockService, ISearchService searchService, IPlaylistService playlistMockService, IPlaylistService playlistService, IDownloadService downloadMockService, IDownloadService downloadService)
    {
        _searchService = searchService;
        _searchMockService = searchMockService;

        _playlistService = playlistService;
        _playlistMockService = playlistMockService;

        _downloadService = downloadService;
        _downloadMockService = downloadMockService;

        updateDependencies(false);
    }

    public ISearchService SearchService { get; private set; }
    public IPlaylistService PlaylistService { get; private set; }
    public IDownloadService DownloadService { get; private set; }

    [MemberNotNull(nameof(SearchService), nameof(PlaylistService), nameof(DownloadService))]
    public void updateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            SearchService = _searchMockService;
            PlaylistService = _playlistMockService;
            DownloadService = _downloadMockService;
        }
        else
        {
            SearchService = _searchService;
            PlaylistService = _playlistService;
            DownloadService = _downloadService;
        }
    }
}