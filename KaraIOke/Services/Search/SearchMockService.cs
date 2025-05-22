using KaraIOke.Models;

namespace KaraIOke.Services.Search;

public class SearchMockService : ISearchService
{

    string _songName = "";

    public void QuerySongs(string songName)
    {
        _songName = songName;
    }

    public async Task<IEnumerable<Song>> GetSongs()
    {
        return await Task.Run(() =>
        {
            Thread.Sleep(2000);
            return Enumerable.Range(1, 10)
            .Select(i => $"{_songName}: Song {i}")
            .Select(str => new Song { title = str, url = "" });
        });
    }

}