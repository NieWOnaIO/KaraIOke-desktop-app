using System.Collections.ObjectModel;
using System.Text.Json;
using KaraIOke.Models;
using Microsoft.Maui.Storage;

namespace KaraIOke.Services.Playlists;

public class PlaylistService : IPlaylistService
{
    private static readonly string FilePath = Path.Combine(
        FileSystem.Current.AppDataDirectory,
        "playlists.json"
    );

    private List<Playlist> _playlists = new();

    public PlaylistService()
    {
        EnsureJsonFileExists();
        LoadPlaylists();
    }

    private void EnsureJsonFileExists()
    {
        if (!File.Exists(FilePath))
        {
            var emptyData = new Dictionary<string, List<Song>>();
            var json = JsonSerializer.Serialize(
                emptyData,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(FilePath, json);
        }
    }

    private void LoadPlaylists()
    {
        var json = File.ReadAllText(FilePath);
        var dict = JsonSerializer.Deserialize<Dictionary<string, List<Song>>>(json) ?? new();

        _playlists = dict.Select(entry => new Playlist(
                entry.Key,
                new ObservableCollection<Song>(entry.Value)
            ))
            .ToList();
    }

    private void SavePlaylists()
    {
        var dict = _playlists.ToDictionary(p => p.Name, p => p.Songs.ToList());

        var json = JsonSerializer.Serialize(
            dict,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }
        );

        File.WriteAllText(FilePath, json);
    }

    public Playlist GetPlaylist(string playlistName)
    {
        var playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);

        if (playlist == null)
            throw new ArgumentException($"Playlist '{playlistName}' not found.");

        return playlist;
    }

    public ObservableCollection<string> GetAllPlaylistsNames()
    {
        return new ObservableCollection<string>(_playlists.Select(p => p.Name));
    }

    public void AddPlaylist(Playlist playlist)
    {
        if (_playlists.Any(p => p.Name == playlist.Name))
            throw new ArgumentException($"Playlist '{playlist.Name}' already exists.");

        _playlists.Add(playlist);
        SavePlaylists();
    }

    public void DeletePlaylist(string playlistName)
    {
        var playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);

        if (playlist == null)
            throw new ArgumentException($"Playlist '{playlistName}' not found.");

        _playlists.Remove(playlist);
        SavePlaylists();
    }

    public void DeleteSong(string playlistName, Song song)
    {
        var playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);
        if (playlist == null)
            throw new ArgumentException($"Playlist '{playlistName}' not found.");

        var existingSong = playlist.Songs.FirstOrDefault(s => s.hash == song.hash);
        if (existingSong == null)
            throw new ArgumentException(
                $"Song with hash '{song.hash}' not found in playlist '{playlistName}'."
            );

        playlist.Songs.Remove(existingSong);
        SavePlaylists();
    }

    public void AddSong(string playlistName, Song song)
    {
        var playlist = _playlists.FirstOrDefault(p => p.Name == playlistName);
        if (playlist == null)
            throw new ArgumentException($"Playlist '{playlistName}' not found.");

        if (playlist.Songs.Any(s => s.hash == song.hash))
            throw new ArgumentException("Song already exists in this playlist.");

        playlist.Songs.Add(song);
        SavePlaylists();
    }
}
