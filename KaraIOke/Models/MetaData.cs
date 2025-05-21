namespace KaraIOke.Models;

public class MetaData
{
    public bool ready { get; set; }
    public string link { get; set; } = string.Empty;
    public string title { get; set; } = string.Empty;
    public string author { get; set; } = string.Empty;
    public string album { get; set; } = string.Empty;
    public string release { get; set; } = string.Empty;
    public int duration { get; set; }
}