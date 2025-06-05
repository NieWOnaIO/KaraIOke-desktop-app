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

    public Playlist GetAsPlaylist()
    {
        return new Playlist(
            "Historia",
            new ObservableCollection<Song>(_history)
        );
    }

    public void RemoveSong(Song song)
    {
        _history.Remove(song);
    }
}
