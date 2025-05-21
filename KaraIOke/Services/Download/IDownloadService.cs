using KaraIOke.Models;

namespace KaraIOke.Services.Download;

public interface IDownloadService
{
    Task QueryDownload(Song song);
}