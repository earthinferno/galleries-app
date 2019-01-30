using GalleriesServer.Models;
using System.Collections.Generic;
using System.Linq;
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

        internal async Task UpdateMediaItem(MediaItem item)
        {
            var dbMediaItem = await _dbContext.MediaItems.FindAsync(item.ID);
            if (dbMediaItem == null)
            {
                throw new RepositoryException(RepositiryExceptionType.MediaItemNotFoundForId, item.ID);
            }
            _dbContext.Entry(dbMediaItem).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteMediaItem(int itemId)
        {
            var dbMediaItem = await _dbContext.MediaItems.FindAsync(itemId);
            if (dbMediaItem == null)
            {
                throw new RepositoryException(RepositiryExceptionType.MediaItemNotFoundForId, itemId);
            }

            _dbContext.Remove(dbMediaItem);
            await _dbContext.SaveChangesAsync();

        }


        //internal async Task<List<MediaItem>> GetMediaItemsForContainer(int containerId)
        //{
        //    var dbMediaItem = await _dbContext.MediaItems.Where(a => a.);
        //    if (dbMediaItem == null)
        //    {
        //        throw new RepositoryException(RepositiryExceptionType.MediaItemNotFoundForId, itemId);
        //    }

        //    _dbContext.Remove(dbMediaItem);
        //    await _dbContext.SaveChangesAsync();

        //}


    }
}
