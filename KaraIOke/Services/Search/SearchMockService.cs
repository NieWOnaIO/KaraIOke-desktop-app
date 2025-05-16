using KaraIOke.Models;

namespace KaraIOke.Services.Search;

public class SearchMockService : ISearchService
{

    string _songName = "";

    public void querySongs(string songName)
    {
        _songName = songName;
    }

    public void queryDownload(Song song) { }

    public async Task<IEnumerable<Song>> getSongs()
    {
        return Enumerable.Range(1, 10)
            .Select(i => $"{_songName}: Song {i}")
            .Select(str => new Song { title = str, url = "" });
    }

}