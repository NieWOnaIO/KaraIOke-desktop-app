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

    public void querySongs(string songName)
    {
        _searchTask = _client.GetAsync($"/v1/search/{songName}")
            .ContinueWith(async responseTask =>
            {
                var response = await responseTask;
                var responseBody = await response.Content.ReadAsStringAsync();
                string responseStr = JsonConvert.DeserializeObject<string>(responseBody) ?? string.Empty;
                _songs = JsonConvert.DeserializeObject<List<Song>>(responseStr) ?? [];
            });
    }

    public void queryDownload(Song song)
    {
        _client.PostAsync($"v1/songs?link={song.url}", null);
    }

    public async Task<IEnumerable<Song>> getSongs()
    {
        await _searchTask;
        return _songs;
    }
}