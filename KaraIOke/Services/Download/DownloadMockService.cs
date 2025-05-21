using KaraIOke.Models;

namespace KaraIOke.Services.Download;

public class DownloadMockService : IDownloadService
{
    public Task QueryDownload(Song song) { return Task.FromResult(0); }
    public SongAudio GetSongAudio(Song song) { return new(); }

}