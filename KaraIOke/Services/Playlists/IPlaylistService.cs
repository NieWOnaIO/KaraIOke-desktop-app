using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public interface IPlaylistService
{
    Playlist GetPlaylist(string playlistName);
    ObservableCollection<string> GetAllPlaylistsNames();
    void DeletePlaylist(string playlistName);
    void AddPlaylist(Playlist playlist);
    void AddSong(string playlistName, Song song);
    void DeleteSong(string playlistName, Song song);
}