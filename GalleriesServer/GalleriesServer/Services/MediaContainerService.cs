using GalleriesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GalleriesServer.Services
{
    public class MediaContainerService
    {

        private readonly GalleriesDbContext _dbContext;
        public MediaContainerService (GalleriesDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        internal async Task<MediaContainer> GetMediaContainerByName(Owner owner, string name)
        {
            //var container = await _dbContext.MediaContainers.Where(a => a.Name == containerName).ToListAsync();
            var container = await _dbContext.MediaContainers.Where(a => a.Owner.ID == owner.ID && a.Name == name).ToListAsync();
            if (container == null || container.Count() == 0)
            {
                return null;
            }
            return container.First();
        }

        internal async Task<MediaContainer> GetMediaContainerById(int mediaContainerId)
        {
            //var container = await _dbContext.MediaContainers.Where(a => a.Name == containerName).ToListAsync();
            var container = await _dbContext.MediaContainers.FindAsync(mediaContainerId);
            if (container == null)
            {
                return null;
            }
            return container;
        }

        internal async Task AddMediaContainer(MediaContainer container)
        {
            _dbContext.MediaContainers.Add(container);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task UpdateMediaContainer(MediaContainer mediaContainer)
        {
            //_dbContext.Entry(mediaContainer).State = EntityState.Modified;
            var currentMediaContainer = await _dbContext.MediaContainers.FindAsync(mediaContainer.ID);
            _dbContext.Entry(currentMediaContainer).CurrentValues.SetValues(mediaContainer);
            await _dbContext.SaveChangesAsync();
        }
        // Not used but may be useful
        #region unused
        //internal async Task<MediaContainer> GetContainerOrNewIfNotExists(Owner owner, string userFolder)
        //{
        //    var container = await GetMediaContainerByName(owner, userFolder);
        //    if (container == null)
        //    {
        //        await AddMediaContainer(new MediaContainer()
        //        {
        //            CreatedDate = DateTime.Now,
        //            Name = userFolder,
        //            Description = "Undefined",
        //            Owner = owner
        //        });
        //        container = await GetMediaContainerByName(owner, userFolder);
        //    }
        //    return container;
        //}

        #endregion

    }
}
