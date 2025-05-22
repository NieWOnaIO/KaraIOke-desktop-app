using KaraIOke.Models;

namespace KaraIOke.Services.Download;

public class DownloadMockService : IDownloadService
{

    private SongAudio _songAudio = new();

    public async Task QueryDownload(Song song)
    {
        var vocalsStream = FileSystem.OpenAppPackageFileAsync("vocals.mp3");
        var noVocalsStream = FileSystem.OpenAppPackageFileAsync("no_vocals.mp3");

        _songAudio = new SongAudio { NoVocals = await noVocalsStream, Vocals = await vocalsStream };
    }
    public SongAudio GetSongAudio(Song song)
    {
        return _songAudio;
    }

}