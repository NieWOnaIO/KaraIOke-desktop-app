using KaraIOke.Models;

namespace KaraIOke.Services.Search;

public interface ISearchService
{
    void QuerySongs(string songName);

    Task<IEnumerable<Song>> GetSongs();
}