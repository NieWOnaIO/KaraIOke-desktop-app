using KaraIOke.Models;
using System.Collections.Generic;

namespace KaraIOke.Services.Search;

public class HistoryService : IHistoryService
{
    private readonly List<Song> _history = new();

    public void Add(Song song)
    {
        if (song == null) return;
        _history.Add(song);
    }

    public IReadOnlyList<Song> GetAll() => _history.AsReadOnly();
}
