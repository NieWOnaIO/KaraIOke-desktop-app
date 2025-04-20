using KaraIOke.Services.Search;
using KaraIOke.Views;

namespace KaraIOke.Services.AppEnvironment;

public class AppEnvironmentService {
    private readonly ISearchService _searchService;
    private readonly ISearchService _searchMockService;

    public AppEnvironmentService(ISearchService searchMockService, ISearchService searchService) {
        _searchService = searchService;
        _searchMockService = searchMockService;
    }

    public ISearchService SearchService { get; private set; }

    public void updateDependencies(bool useMockServices)
    {
        if (useMockServices)
        {
            SearchService = _searchMockService;
        } else {
            SearchService = _searchService;
        }
    }
}