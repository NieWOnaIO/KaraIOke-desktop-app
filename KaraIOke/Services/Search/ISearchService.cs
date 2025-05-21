using KaraIOke.Models;

namespace KaraIOke.Services.Search;

public interface ISearchService
{
    void querySongs(string songName);

    Task<IEnumerable<Song>> getSongs();
}