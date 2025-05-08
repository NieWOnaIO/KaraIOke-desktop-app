using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public class PlaylistMockService : IPlaylistService
{
    private Dictionary<string, List<Song>> playlists = new Dictionary<string, List<Song>>();

    public PlaylistMockService()
    {
        for (int i = 0; i < 5; i++)
        {
            string playlistName = $"Mock playlist {i}";
            var songs = Enumerable.Range(1, 5)
                .Select(i => new Song { Name = $"Mock song {i}" })
                .ToList();
            playlists.Add(playlistName, songs);
        }
    }
    public Playlist GetPlaylist(string playlistName)
    {
        var songs = playlists[playlistName];
        return new Playlist(playlistName, new ObservableCollection<Song>(songs));
    }

    public ObservableCollection<string> GetAllPlaylistsNames()
    {
        var names = playlists.Keys.ToList();
        return new ObservableCollection<string>(names);
    }

    public void DeletePlaylist(string playlistName)
    {
        playlists.Remove(playlistName);
    }

    public void DeleteSong(string playlistName, Song song)
    {
        var songs = playlists[playlistName];
        songs.Remove(song);
    }
}