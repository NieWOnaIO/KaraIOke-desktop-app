using KaraIOke.Models;

namespace KaraIOke.Services.History;

public interface IHistoryService
{
    void Add(Song song);
    public IReadOnlyList<Song> GetAll();
    Playlist GetAsPlaylist();
}
