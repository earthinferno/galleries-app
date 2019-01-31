using Microsoft.EntityFrameworkCore;

namespace GalleriesServer.Models
{
    public interface IGalleriesDbContext
    {
        DbSet<MediaContainer> MediaContainers { get; set; }
        DbSet<MediaItem> MediaItems { get; set; }
        DbSet<Owner> Owners { get; set; }
    }
}