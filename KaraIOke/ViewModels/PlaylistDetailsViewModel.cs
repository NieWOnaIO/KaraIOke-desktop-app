using System.ComponentModel;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;
using System.Windows.Input;
using KaraIOke.Models;
using System.Collections.ObjectModel;

namespace KaraIOke.ViewModels;

public partial class PlaylistDetailsViewModel : INotifyPropertyChanged
{
    protected readonly NavigationService _navigationService;
    protected readonly AppEnvironmentService _appEnvironmentService;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand GoToMain { private set; get; }
    public ICommand GoToPlayer { private set; get; }
    public ICommand DeleteSong { private set; get; }

    public Playlist? Playlist { get; private set; }

    public ObservableCollection<Song> Songs
    {
        get => Playlist?.Songs ?? new ObservableCollection<Song>();
    }

    public PlaylistDetailsViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered.");
        _appEnvironmentService = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered.");

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

        DeleteSong = new Command<Song>(
            execute: async (song) =>
            {
                if (song != null)
                {
                    await Task.Run(() =>
                    {
                        var playlist = Playlist ?? throw new ArgumentException("null Playlist");
                        _appEnvironmentService.PlaylistService.DeleteSong(playlist.Name, song);
                        loadData(Playlist.Name);
                    });

                }
            }
        );
    }

    public void loadData(string playlistName)
    {
        Playlist = _appEnvironmentService.PlaylistService.GetPlaylist(playlistName);

        OnPropertyChanged(nameof(Playlist));
        OnPropertyChanged(nameof(Songs));
    }

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}