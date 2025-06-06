using System.Text;
using KaraIOke.Models;
using SubtitlesParser.Classes;
using System.Security.Cryptography;

namespace KaraIOke.Services.Download;

public class SongAudio
{
    public Stream Vocals { get; set; }
    public Stream NoVocals { get; set; }

    public List<SubtitleItem> Lyrics { get; set; } = [];
}

public class DownloadService : IDownloadService
{
    private Dictionary<string, Task> _songDownloads = new();
    private Dictionary<string, string> _songHashes = new();
    private Dictionary<string, SongAudio> _songAudios = new();
    private HashAlgorithm algorithm = SHA256.Create();

    private HttpClient _client = new HttpClient();
    public DownloadService()
    {
        _client.BaseAddress = new Uri("http://localhost:8000/");
    }

    public SongAudio GetSongAudio(Song song)
    {
        if (_songHashes.ContainsKey(song.url))
            song.hash = _songHashes[song.url];
        return _songAudios[song.url];
    }

    public Task QueryDownload(Song song)
    {
        if (_songDownloads.ContainsKey(song.url))
            return _songDownloads[song.url];

        var task = downloadFlow(song);

        _songDownloads.Add(song.url, task);

        return task;
    }

    private async Task downloadFlow(Song song)
    {
        var filePath = Path.Combine(
            FileSystem.Current.AppDataDirectory,
            song.url.Remove(0, "https:\\www.youtube.com\\watch?v=".Count())
        );
        if (File.Exists(filePath))
        {
            var vocalsPath = Path.Combine(filePath, "vocals.mp3");
            var noVocalsPath = Path.Combine(filePath, "no_vocals.mp3");

            var vocalsStream = FileSystem.OpenAppPackageFileAsync(vocalsPath);
            var noVocalsStream = FileSystem.OpenAppPackageFileAsync(noVocalsPath);

            _songAudios.Add(song.url, new SongAudio { Vocals = await vocalsStream, NoVocals = await noVocalsStream });
            return;
        }

        var response = await _client.PostAsync($"v1/process_song?link={song.url}", null);
        var songID = await response.Content.ReadAsAsync<SongID>();
        song.hash = songID.song_id;
        _songHashes.Add(song.url, song.hash);

        await waitForSong(song);
    }

    private async Task<bool> pollSong(Song song)
    {
        var response = await _client.GetAsync($"v1/songinfo/{song.hash}");
        var metaData = await response.Content.ReadAsAsync<MetaData>();
        return metaData.ready;
    }

    private async Task<bool> pollLyrics(Song song)
    {
        var response = await _client.GetAsync($"v1/lyricsinfo/{song.hash}");
        var metaData = await response.Content.ReadAsAsync<MetaData>();
        return metaData.ready;
    }

    private async Task<Stream> getAudio(Song song, string path)
    {
        var response = await _client.GetAsync($"v1/{path}/{song.hash}");
        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<List<SubtitleItem>> waitForLyrics(Song song)
    {
        if (_songHashes.ContainsKey(song.url))
            song.hash = _songHashes[song.url];


        var filePath = Path.Combine(
            FileSystem.Current.AppDataDirectory,
            song.url.Remove(0, "https:\\www.youtube.com\\watch?v=".Count())
        );
        var parser = new SubtitlesParser.Classes.Parsers.SrtParser();

        if (File.Exists(Path.Join(filePath, "lyrics.srt")))
        {
            var _lyricsPath = Path.Combine(filePath, "lyrics.srt");

            var lyricsStream1 = FileSystem.OpenAppPackageFileAsync(_lyricsPath);
            return parser.ParseStream(await lyricsStream1, Encoding.UTF8);
        }

        while (!await pollLyrics(song))
        {
            Thread.Sleep(1000);
        }

        var lyricsStream = await getAudio(song, "lyrics");

        Directory.CreateDirectory(filePath);
        var lyricsPath = Path.Combine(filePath, "lyrics.srt");

        using (var file = File.Create(lyricsPath))
        {
            lyricsStream.Seek(0, SeekOrigin.Begin);
            lyricsStream.CopyTo(file);
        }

        return parser.ParseStream(lyricsStream, Encoding.UTF8);
    }

    private async Task waitForSong(Song song)
    {
        if (_songHashes.ContainsKey(song.url))
            song.hash = _songHashes[song.url];
        while (!await pollSong(song))
        {
            Thread.Sleep(1000);
        }

        var vocals = getAudio(song, "song_vocals");
        var noVocals = getAudio(song, "song_no_vocals");
        _songAudios.Add(song.url, new SongAudio { Vocals = await vocals, NoVocals = await noVocals });
    }

    public async Task SaveSong(Song song)
    {
        await QueryDownload(song);

        var audio = GetSongAudio(song);
        var filePath = Path.Combine(
            FileSystem.Current.AppDataDirectory,
            song.url.Remove(0, "https:\\www.youtube.com\\watch?v=".Count())
        );


        Directory.CreateDirectory(filePath);
        var vocalsPath = Path.Combine(filePath, "vocals.mp3");
        var noVocalsPath = Path.Combine(filePath, "no_vocals.mp3");

        using (var file = File.Create(vocalsPath))
        {
            audio.Vocals.Seek(0, SeekOrigin.Begin);
            audio.Vocals.CopyTo(file);
        }
        using (var file = File.Create(noVocalsPath))
        {
            audio.NoVocals.Seek(0, SeekOrigin.Begin);
            audio.NoVocals.CopyTo(file);
        }
    }
}