using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Services.Playlists;

public class PlaylistMockService : IPlaylistService
{
    public Playlist GetPlaylist(string playlistName)
    {
        var songs = Enumerable.Range(1, 5)
            .Select(i => new Song { Name = $"Mock song {i}" })
            .ToList();
        return new Playlist(playlistName, new ObservableCollection<Song>(songs));
    }
}