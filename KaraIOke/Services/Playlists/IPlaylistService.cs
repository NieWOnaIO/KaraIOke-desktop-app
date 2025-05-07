using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public interface IPlaylistService {
    Playlist GetPlaylist(string playlistName);
}