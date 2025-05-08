using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public class PlaylistService : IPlaylistService
{
    public Playlist GetPlaylist(string playlistName)
    {
        throw new NotImplementedException();
    }

    public ObservableCollection<string> GetAllPlaylistsNames()
    {
        throw new NotImplementedException();
    }

    public void DeletePlaylist(string playlistName)
    {
        throw new NotImplementedException();
    }

    public void DeleteSong(string playlistName, Song song)
    {
        throw new NotImplementedException();
    }
}