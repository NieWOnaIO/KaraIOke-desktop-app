using System.Threading.Tasks;
using Newtonsoft.Json;
using KaraIOke.Models;
using Windows.ApplicationModel.VoiceCommands;

namespace KaraIOke.Services.Search;

public class SearchService : ISearchService
{

    private HttpClient _client = new HttpClient();
    private Task _searchTask = Task.FromResult(0);
    private List<Song> _songs = [];

    public SearchService()
    {
        _client.BaseAddress = new Uri("http://localhost:8000/");
    }

    public void QuerySongs(string songName)
    {
        _searchTask = requestSongs(songName);
    }

    private async Task requestSongs(string songName)
    {
        var response = await _client.GetAsync($"/v1/search/{songName}");
        _songs = await response.Content.ReadAsAsync<List<Song>>();
    }

    public async Task<IEnumerable<Song>> GetSongs()
    {
        await _searchTask;
        return _songs;
    }
}