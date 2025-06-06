using KaraIOke.Models;
using SubtitlesParser.Classes;

namespace KaraIOke.Services.Download;

public interface IDownloadService
{
    Task QueryDownload(Song song);
    Task SaveSong(Song song);
    SongAudio GetSongAudio(Song song);
    Task<List<SubtitleItem>> waitForLyrics(Song song);
}