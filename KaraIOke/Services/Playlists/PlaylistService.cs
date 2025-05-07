using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public class PlaylistService : IPlaylistService
{
    public Playlist GetPlaylist(string playlistName)
    {
        //TODO : Implement the logic to get the playlist 
        ObservableCollection<Song> playlist = new ObservableCollection<Song>();
        return new Playlist(playlistName, playlist);
    }
}