using KaraIOke.Models;
using System.Collections.ObjectModel;

namespace KaraIOke.Services.History;

public class HistoryService : IHistoryService
{
    private readonly List<Song> _history = new();

    public void Add(Song song)
    {
        if (song == null) return;
        _history.Add(song);
    }

    public IReadOnlyList<Song> GetAll() => _history.AsReadOnly();
    public Playlist GetAsPlaylist()
    {
        return new Playlist(
            "History",
            new ObservableCollection<Song>(_history)
        );
    }
}
