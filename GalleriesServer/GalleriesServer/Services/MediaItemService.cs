using GalleriesServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleriesServer.Services
{
    public class MediaItemService
    {
        private readonly GalleriesDbContext _dbContext;
        public MediaItemService(GalleriesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal void AddMediaItem(MediaContainer container, MediaItem item)
        {
            if (container.MediaItems == null)
            {
                container.MediaItems = new List<MediaItem>();
            }

            container.MediaItems.Add(item);
        }
    }
}
