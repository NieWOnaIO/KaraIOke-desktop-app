using KaraIOke.Models.Song;

namespace KaraIOke.Services.Search;

public class SearchMockService : ISearchService
{

    string _songName;

    public void querySongs(string songName)
    {
        _songName = songName;
    }

    public IEnumerable<Song> getSongs() {
        return Enumerable.Range(1, 10)
            .Select(i => $"{_songName}: Song {i}")
            .Select(str => new Song {Name = str});
    }

}