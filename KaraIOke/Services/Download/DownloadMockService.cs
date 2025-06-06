using System.Text;
using SubtitlesParser.Classes;
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

        // Thread.Sleep(10000);
    }
    public SongAudio GetSongAudio(Song song)
    {
        return _songAudio;
    }

    public async Task<List<SubtitleItem>> waitForLyrics(Song song)
    {
        Thread.Sleep(2000);
        var lyricsStream = FileSystem.OpenAppPackageFileAsync("lyrics.srt");

        var parser = new SubtitlesParser.Classes.Parsers.SrtParser();
        var lyrics = parser.ParseStream(await lyricsStream, Encoding.UTF8);

        return lyrics;
    }
    public Task SaveSong(Song song)
    {
        return Task.FromResult(0);
    }

}