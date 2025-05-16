using KaraIOke.Models;

namespace KaraIOke.Services.Search;

public interface ISearchService
{
    void querySongs(string songName);
    void queryDownload(Song song);

    Task<IEnumerable<Song>> getSongs();
}