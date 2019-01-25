using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace GalleriesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleriesController : Controller
    {
        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;
        private readonly MediaContainerService _containerService;

        public GalleriesController(GalleriesDbContext galleriesDbContext, OwnerService ownerService, MediaContainerService containerService)
        {
            _dbContext = galleriesDbContext;
            _ownerService = ownerService;
            _containerService = containerService;
        }

        /// <summary>
        /// Return all galleries for the supplied UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<MediaContainer>>> GetGalleries(string userId)
        {
            var gallery = await _dbContext.MediaContainers.Where(a => a.Owner.ExternalUserId == userId).ToListAsync();
            if (gallery == null)
            {
                return NotFound();
            }

            // response code 200
            return gallery;
        }

        /// <summary>
        /// Create a new gallery.
        /// </summary>
        /// <param name="galleryItem"></param>
        /// <returns></returns>
        // POST: api/Galleries
        [HttpPost]
        public async Task<ActionResult<MediaContainer>> PostGallery(Gallery galleryItem)
        {
            var owner = await _ownerService.GetOwner(galleryItem.UserId);
            if (owner == null)
            {
                return BadRequest();
            }

            _dbContext.MediaContainers.Add(new MediaContainer() {
                CreatedDate = galleryItem.CreatedDate,
                Name = galleryItem.Name,
                Description = galleryItem.Description,
                Owner = owner
            });
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetGalleries", new { userId = galleryItem.UserId }, galleryItem);
        }

        /// <summary>
        /// Update the gallery record for a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gallery"></param>
        /// <returns></returns>
        // PUT: api/Galleries/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutGallery(int id, MediaContainer gallery)
        {
            if (id != gallery.ID)
            {
                //return NotFound(id);
                BadRequest();
            }
            try
            {
                await _containerService.UpdateMediaContainer(gallery);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a gallery for a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Galleries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGallery(int id)
        {
            var gallery = await _dbContext.MediaContainers.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            _dbContext.MediaContainers.Remove(gallery);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
