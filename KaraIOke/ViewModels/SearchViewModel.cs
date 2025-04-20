using System.Collections;
using System.Windows.Input;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;

namespace KaraIOke.ViewModels;

public partial class SearchViewModel : ISearchBar
{
    // protected readonly NavigationService _navigationService;
    // protected readonly AppEnvironmentService _appEnvironmentService;
    IEnumerable _songs;
    public IEnumerable Songs { get { return _songs; } private set { SetProperty(ref _songs, value); } }
    public SearchViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    // public SearchViewModel(IServiceProvider serviceProvider) 
    {
        // _navigationService = serviceProvider.GetService<NavigationService>();
        // _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>();

        GoToMain = new Command(
            execute: async () =>
            {
                await _navigationService.PopPage();
            }
        );
    }

    public void loadData()
    {
        Songs = _appEnvironmentService.SearchService.getSongs();
    }

    public ICommand GoToMain { private set; get; }
}