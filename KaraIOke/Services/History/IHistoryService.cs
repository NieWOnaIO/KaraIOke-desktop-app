using KaraIOke.Models;

namespace KaraIOke.Services.History;

public interface IHistoryService
{
    void Add(Song song);
    Playlist GetAsPlaylist();
    public void RemoveSong(Song song);
}
