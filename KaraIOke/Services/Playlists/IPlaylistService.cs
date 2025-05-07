using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public interface IPlaylistService {
    Playlist GetPlaylist(string playlistName);
    ObservableCollection<string> GetAllPlaylistsNames();
}