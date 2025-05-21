using System.Threading.Tasks;
using Newtonsoft.Json;
using KaraIOke.Models;
using Windows.ApplicationModel.VoiceCommands;

namespace KaraIOke.Services.Download;

public class SongAudio
{
    public byte[] Vocals { get; set; } = [];
    public byte[] NoVocals { get; set; } = [];
}

public class DownloadService : IDownloadService
{
    private Dictionary<string, Task> _songDownloads = new();
    private Dictionary<string, SongAudio> _songAudios = new();

    private HttpClient _client = new HttpClient();
    public DownloadService()
    {
        _client.BaseAddress = new Uri("http://localhost:8000/");
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
        var response = await _client.PostAsync($"v1/process_song?link={song.url}", null);
        var songID = await response.Content.ReadAsAsync<SongID>();
        song.hash = songID.song_id;

        await waitForSong(song);
    }

    private async Task<bool> pollSong(Song song)
    {
        var response = await _client.GetAsync($"v1/songinfo/{song.hash}");
        var metaData = await response.Content.ReadAsAsync<MetaData>();
        return metaData.ready;
    }

    private async Task<byte[]> getAudio(Song song, string path)
    {
        var response = await _client.GetAsync($"v1/{path}/{song.hash}");
        return await response.Content.ReadAsByteArrayAsync();
    }

    private async Task waitForSong(Song song)
    {
        while (!await pollSong(song))
        {
            Thread.Sleep(1000);
        }

        var vocals = getAudio(song, "song_vocals");
        var noVocals = getAudio(song, "song_no_vocals");

        _songAudios.Add(song.hash, new SongAudio { Vocals = await vocals, NoVocals = await noVocals });
    }
}