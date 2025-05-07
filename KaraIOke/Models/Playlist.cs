using System.Collections.ObjectModel;
using KaraIOke.Models;

namespace KaraIOke.Models;

public class Playlist : BindableObject
{
    public string Name { get; set; }

    public ObservableCollection<Song> Songs { get; set; }

    public Playlist(string name, ObservableCollection<Song> songs)
    {
        Name = name;
        Songs = songs;
    }
}