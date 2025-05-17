using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;

namespace KaraIOke.ViewModels;
public partial class ISearchBar : INotifyPropertyChanged
{
    protected readonly NavigationService _navigationService;
    protected readonly AppEnvironmentService _appEnvironmentService;
    public event PropertyChangedEventHandler? PropertyChanged;

    string songName = "";
    public string SongName
    {
        set { SetProperty(ref songName, value); }
        get { return songName; }
    }

    public ISearchBar(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered");
        _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered");

        SearchForSong = new Command(
            execute: async () =>
            {
                _appEnvironmentService.SearchService.querySongs(SongName);
                await _navigationService.PushSearchSong();
            }
        );
    }

    public ICommand SearchForSong { private set; get; }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void UpdateProperty<T>(ref T storage, [CallerMemberName] string propertyName = "")
    {
        OnPropertyChanged(propertyName);
    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}