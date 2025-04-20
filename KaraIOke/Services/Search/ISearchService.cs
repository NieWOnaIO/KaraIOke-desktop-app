using KaraIOke.Models.Song;

namespace KaraIOke.Services.Search;

public interface ISearchService {
    void querySongs(string songName);

    IEnumerable<Song> getSongs();
}