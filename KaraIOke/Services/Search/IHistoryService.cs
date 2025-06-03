using KaraIOke.Models;
using System.Collections.Generic;

namespace KaraIOke.Services.Search;

public interface IHistoryService
{
    void Add(Song song);
    public IReadOnlyList<Song> GetAll();
}
