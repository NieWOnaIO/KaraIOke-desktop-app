using System.Collections;
using System.Windows.Input;

namespace KaraIOke.ViewModels;

public partial class SearchViewModel : ISearchBar
{
    IEnumerable _songs;
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
            execute: async (object? song) =>
            {
                await _navigationService.PushPlayer();
            }
        );

        AddToPlaylist = new Command(
            execute: async () =>
            {
                await _navigationService.PushAdding();
            }
        );
    }

    public void loadData()
    {
        Songs = _appEnvironmentService.SearchService.getSongs();
    }

    public ICommand GoToMain { private set; get; }
    public ICommand GoToPlayer { private set; get; }
    public ICommand AddToPlaylist { private set; get; }
}