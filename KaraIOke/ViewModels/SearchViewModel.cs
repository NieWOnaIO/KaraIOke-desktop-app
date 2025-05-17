using System.Collections;
using System.Threading.Tasks;
using System.Windows.Input;
using KaraIOke.Models;

namespace KaraIOke.ViewModels;

public partial class SearchViewModel : ISearchBar
{
    IEnumerable _songs = Enumerable.Empty<Song>();
    public IEnumerable Songs { get { return _songs; } private set { SetProperty(ref _songs, value); } }
    public SearchViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        GoToMain = new Command(
            execute: async () =>
            {
                await _navigationService.PopPage();
            }
        );

        GoToPlayer = new Command(
            execute: async (object song) =>
            {
                _appEnvironmentService.SearchService.queryDownload((Song)song);
                await _navigationService.PushPlayer();
            }
        );

        AddToPlaylist = new Command(
            execute: async (object song) =>
            {
                _appEnvironmentService.SearchService.queryDownload((Song)song);
                await _navigationService.PushAdding();
            }
        );
    }

    public async Task loadData()
    {
        Songs = Enumerable.Empty<Song>();
        Songs = await _appEnvironmentService.SearchService.getSongs();
    }

    public ICommand GoToMain { private set; get; }
    public ICommand GoToPlayer { private set; get; }
    public ICommand AddToPlaylist { private set; get; }
}