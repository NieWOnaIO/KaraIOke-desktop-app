using System.Text;
using KaraIOke.Models;

namespace KaraIOke.Services.Download;

public class DownloadMockService : IDownloadService
{

    private SongAudio _songAudio = new();

    public async Task QueryDownload(Song song)
    {
        var vocalsStream = FileSystem.OpenAppPackageFileAsync("vocals.mp3");
        var noVocalsStream = FileSystem.OpenAppPackageFileAsync("no_vocals.mp3");
        var lyricsStream = FileSystem.OpenAppPackageFileAsync("lyrics.srt");
        
        var parser = new SubtitlesParser.Classes.Parsers.SrtParser();
        var lyrics = parser.ParseStream(await lyricsStream, Encoding.UTF8);

        _songAudio = new SongAudio { NoVocals = await noVocalsStream, Vocals = await vocalsStream, Lyrics = lyrics };

        // Thread.Sleep(10000);
    }
    public SongAudio GetSongAudio(Song song)
    {
        return _songAudio;
    }

}