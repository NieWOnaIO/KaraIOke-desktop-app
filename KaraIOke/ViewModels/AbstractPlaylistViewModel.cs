using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;

namespace KaraIOke.ViewModels;

public abstract class AbstractPlaylistViewModel : INotifyPropertyChanged
{
    protected readonly NavigationService _navigationService;
    protected readonly AppEnvironmentService _appEnvironmentService;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand GoToMain { private set; get; }

    public ObservableCollection<string> PlaylistsNames { get; private set; }

    public AbstractPlaylistViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered.");
        _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered.");

        GoToMain = new Command(
            execute: async () =>
            {
                await _navigationService.PopPage();
            }
        );

        PlaylistsNames = new ObservableCollection<string>();
    }

    public void loadData()
    {
        PlaylistsNames = _appEnvironmentService.PlaylistService.GetAllPlaylistsNames();

        OnPropertyChanged(nameof(PlaylistsNames));
    }

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}