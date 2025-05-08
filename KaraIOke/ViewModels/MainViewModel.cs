using System.Windows.Input;

namespace KaraIOke.ViewModels;

public partial class MainViewModel : ISearchBar
{
    public ICommand GoToPlaylistList { private set; get; }

    public MainViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        GoToPlaylistList = new Command(
            execute: async () =>
            {
                await _navigationService.PushPlaylistList();
            }
        );
    }
}