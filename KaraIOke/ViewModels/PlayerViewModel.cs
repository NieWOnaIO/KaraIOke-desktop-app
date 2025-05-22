using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KaraIOke.Models;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Download;
using KaraIOke.Services.Navigation;
using Plugin.Maui.Audio;

namespace KaraIOke.ViewModels;

public partial class PlayerViewModel : INotifyPropertyChanged
{
    private const string PlayPng = "play.png";
    private const string PausePng = "pause.png";
    protected readonly NavigationService _navigationService;
    protected readonly IDownloadService _downloadService;

    private readonly IAudioManager _audioManager;

    private Song? _song;
    private IAudioPlayer? _noVocalsPlayer;
    private IAudioPlayer? _vocalsPlayer;

    private string _playButtonSource;
    public string PlayButtonSource
    {
        get => _playButtonSource;
        private set => SetProperty(ref _playButtonSource, value);
    }

    private bool _readyToPlay;

    public bool ReadyToPlay
    {
        get => _readyToPlay;
        private set => SetProperty(ref _readyToPlay, value);
    }

    private bool _isPlayingPlaylist;

    public bool IsPlayingPlaylist
    {
        get => _isPlayingPlaylist;
        private set => SetProperty(ref _isPlayingPlaylist, value);
    }

    private void resetState()
    {
        ReadyToPlay = false;
        IsPlayingPlaylist = false;

        PlayButton = _play;
        PlayButtonSource = PlayPng;
    }

    public async Task SetSong(Song song)
    {
        resetState();
        _song = song;

        await _downloadService.QueryDownload(song);

        var songAudio = _downloadService.GetSongAudio(_song);

        _noVocalsPlayer = _audioManager.CreatePlayer(songAudio.NoVocals);
        _vocalsPlayer = _audioManager.CreatePlayer(songAudio.Vocals);

        ReadyToPlay = true;
    }

    public PlayerViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered");
        var environment = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered");
        _downloadService = environment.DownloadService;

        GoBack = new Command(
            execute: async () =>
            {
                await _navigationService.PopPage();
            }
        );

        _play = new Command(
            execute: () =>
            {
                if (_vocalsPlayer is not null && _noVocalsPlayer is not null)
                {
                    _vocalsPlayer.Play();
                    _noVocalsPlayer.Play();

                    PlayButton = _pause;
                    PlayButtonSource = PausePng;
                }
            }
        );

        _pause = new Command(
            execute: () =>
            {
                if (_vocalsPlayer is not null && _noVocalsPlayer is not null)
                {
                    _vocalsPlayer.Pause();
                    _noVocalsPlayer.Pause();

                    PlayButton = _play;
                    PlayButtonSource = PlayPng;
                }
            }
        );

        PlayButton = _play;

        _audioManager = serviceProvider.GetService<IAudioManager>() ?? throw new InvalidOperationException("AudioManager not registered");
    }

    public ICommand GoBack { private set; get; }
    private ICommand _playButton;
    public ICommand PlayButton
    {
        get => _playButton;
        private set => SetProperty(ref _playButton, value);
    }
    private ICommand _play;
    private ICommand _pause;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}