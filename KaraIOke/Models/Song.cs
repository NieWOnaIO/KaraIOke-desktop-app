namespace KaraIOke.Models;

public class Song : BindableObject
{
    public string title { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
    public string hash { get; set; } = string.Empty;

}