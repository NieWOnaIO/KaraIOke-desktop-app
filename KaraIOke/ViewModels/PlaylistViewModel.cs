using System.ComponentModel;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;
using System.Windows.Input;
using KaraIOke.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KaraIOke.ViewModels;

public partial class PlaylistViewModel : INotifyPropertyChanged
{
    protected readonly NavigationService _navigationService;
    protected readonly AppEnvironmentService _appEnvironmentService;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand GoToMain { private set; get; }
    public ICommand GoToPlayer {private set; get; }

    public Playlist? Playlist { get; private set; }

    public ObservableCollection<Song> Songs
    {
        get => Playlist?.Songs ?? new ObservableCollection<Song>();
    }

    public PlaylistViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered.");
        _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered.");

        GoToMain = new Command(
            execute: async () =>
            {
                Debug.WriteLine("GO TO MAIN");
                await _navigationService.PopPage();
            }
        );

        GoToPlayer = new Command(
            execute: async(object? song) =>
            {
                Debug.WriteLine("GO TO PLAYER");
                await _navigationService.PushPlayer();
            }
        );

        //TODO: delete after implementing PlaylistList
        LoadPlaylist("DefaultPlaylist");
    }

    public void LoadPlaylist(string playlistName)
    {
        Playlist = _appEnvironmentService.PlaylistService.GetPlaylist(playlistName);

        OnPropertyChanged(nameof(Playlist));
    }

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}