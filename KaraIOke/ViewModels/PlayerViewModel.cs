using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KaraIOke.Models;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Download;
using KaraIOke.Services.Navigation;
using KaraIOke.Views.Templates;
using Plugin.Maui.Audio;

namespace KaraIOke.ViewModels;

public class DoubleToTimeStrConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        return TimeSpan.FromSeconds((double)value).ToString("mm':'ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        return 0.0;
    }
}

public partial class PlayerViewModel : INotifyPropertyChanged
{
    private Mutex _mutex = new();
    private const string PlayPng = "play.png";
    private const string PausePng = "pause.png";
    protected readonly NavigationService _navigationService;
    protected readonly IDownloadService _downloadService;

    private readonly IAudioManager _audioManager;

    private Song? _song;
    public Song Song
    {
        get => _song ?? new Song();
        private set => SetProperty(ref _song, value);
    }

    public string SongName
    {
        get => _song?.title ?? string.Empty;
    }

    private IAudioPlayer? _noVocalsPlayer;
    private IAudioPlayer? _vocalsPlayer;

    private double _audioPosition;
    public double AudioPosition
    {
        get => _audioPosition;
        private set => SetProperty(ref _audioPosition, value);
    }
    private double _audioLength;
    public double AudioLength
    {
        get => _audioLength;
        private set => SetProperty(ref _audioLength, value);
    }
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

    public bool ForwardButtonEnabled
    {
        get => _isPlayingPlaylist && _readyToPlay && _playlistPosition < _playlist?.Songs.Count() - 1;
    }

    private Playlist? _playlist;
    private int _playlistPosition;

    private void resetState()
    {
        pauseSong();

        ReadyToPlay = false;
        _isPlayingPlaylist = false;
        OnPropertyChanged(nameof(ForwardButtonEnabled));

        PlayButton = _play;
        PlayButtonSource = PlayPng;

        AudioPosition = 0.0;
        AudioLength = 0.0;
    }

    private CancellationTokenSource? _cancellationTokenSource;

    public CancellationTokenSource? GetTokenSource()
    {
        return _cancellationTokenSource;
    }
    public CancellationToken GenerateNewToken()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        return _cancellationTokenSource.Token;
    }
    private void pauseSong()
    {
        if (_noVocalsPlayer is not null)
            _noVocalsPlayer.Pause();

        if (_vocalsPlayer is not null)
            _vocalsPlayer.Pause();
    }

    public async Task SetData(Song song, Playlist? playlist, CancellationToken cancellationToken)
    {
        resetState();
        _song = song;
        OnPropertyChanged(nameof(SongName));

        _playlist = playlist;
        if (playlist != null)
        {
            _isPlayingPlaylist = true;
            _playlistPosition = playlist.Songs.IndexOf(song);
        }
        OnPropertyChanged(nameof(ForwardButtonEnabled));

        await _downloadService.QueryDownload(song);
        if (cancellationToken.IsCancellationRequested)
            return;

        var songAudio = _downloadService.GetSongAudio(_song);

        _noVocalsPlayer = _audioManager.CreatePlayer(songAudio.NoVocals);
        _vocalsPlayer = _audioManager.CreatePlayer(songAudio.Vocals);

        _noVocalsPlayer.PlaybackEnded += (o, e) =>
        {
            if (!_isPlayingPlaylist || !ForwardButtonEnabled)
            {
                _pause.Execute(null);
            }
            else
            {
                ForwardButton.Execute(null);
            }
        };

        ReadyToPlay = true;
        OnPropertyChanged(nameof(ForwardButtonEnabled));
    }

    public PlayerViewModel(IServiceProvider serviceProvider)
    {
        _navigationService = serviceProvider.GetService<NavigationService>() ?? throw new InvalidOperationException("NavigationService is not registered");
        var environment = serviceProvider.GetService<AppEnvironmentService>() ?? throw new InvalidOperationException("AppEnvironmentService is not registered");
        _downloadService = environment.DownloadService;

        RewindButton = new Command(
            execute: () =>
            {
                _mutex.WaitOne();
                if (_playlist != null && _audioPosition < 2 && _playlistPosition > 0)
                {
                    if (_cancellationTokenSource != null)
                    {
                        _cancellationTokenSource.Cancel();
                    }
                    Task.Run(() => SetData(_playlist.Songs[_playlistPosition - 1], _playlist, GenerateNewToken()));
                    _mutex.ReleaseMutex();
                    return;
                }

                if (_vocalsPlayer is not null && _noVocalsPlayer is not null)
                {
                    _noVocalsPlayer.Seek(0);
                    _vocalsPlayer.Seek(0);
                    AudioPosition = 0.0;
                }
                _mutex.ReleaseMutex();
            }
        );

        ForwardButton = new Command(
            execute: () =>
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }
                Task.Run(() => SetData(_playlist.Songs[_playlistPosition + 1], _playlist, GenerateNewToken()));
            }
        );

        GoBack = new Command(
            execute: async () =>
            {
                resetState();
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

                    AudioLength = _noVocalsPlayer.Duration;

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

        Task.Run(() =>
        {
            while (true)
            {
                _mutex.WaitOne();
                if (_noVocalsPlayer is not null && _noVocalsPlayer.IsPlaying)
                {
                    AudioPosition = _noVocalsPlayer.CurrentPosition;
                }
                _mutex.ReleaseMutex();

                Thread.Sleep(100);
            }
        });

        _audioManager = serviceProvider.GetService<IAudioManager>() ?? throw new InvalidOperationException("AudioManager not registered");
    }

    public void OnAudioSliderValueChanged(double value)
    {
        _mutex.WaitOne();
        if (_vocalsPlayer is not null && _noVocalsPlayer is not null)
        {
            _noVocalsPlayer.Seek(value);
            _vocalsPlayer.Seek(value);
        }
        _mutex.ReleaseMutex();
    }

    public void VocalVolumeChanged(double value)
    {
        if (_vocalsPlayer is not null)
        {
            _vocalsPlayer.Volume = value;
        }
    }

    public void NoVocalVolumeChanged(double value)
    {
        if (_noVocalsPlayer is not null)
        {
            _noVocalsPlayer.Volume = value;
        }
    }

    public ICommand RewindButton { private set; get; }
    public ICommand ForwardButton { private set; get; }

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