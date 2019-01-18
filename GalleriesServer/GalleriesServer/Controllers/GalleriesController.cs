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

        public GalleriesController(GalleriesDbContext galleriesDbContext, OwnerService ownerService)
        {
            _dbContext = galleriesDbContext;
            _ownerService = ownerService;
        }

        /*
         * DO not expose this. This is insecure.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaContainer>>> GetGalleries()
        {
            return await _dbContext.MediaContainers.ToListAsync();
        }*/

        /// <summary>
        /// Return all galleries for the supplied UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<MediaContainer>>> GetGalleries(string userId)
        {
            /*
            var owner = _dbContext.Owner.Where(a => a.ExternalUserId == userId) as Owner;
            if (owner == null)
            {
                return NotFound();
            }
            if (_dbContext.MediaContainers.Where(a => a.Owner.ID == owner.ID).ToList().Count() == 0)
            {
                return NotFound();
            }

            //var gallery = await _dbContext.MediaContainers.Where(a => a.Owner.ID == owner.ID).ToListAsync(); */
            var gallery = await _dbContext.MediaContainers.Where(a => a.Owner.ExternalUserId == userId).ToListAsync();
            if (gallery == null)
            {
                return NotFound();
            }

            return gallery;
        }

/*
        /// <summary>
        /// Get a gallery specific to the provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Galleries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaContainer>> GetGallery(long id)
        {
            var gallery = await _dbContext.MediaContainers.FindAsync(id);

            if (gallery == null)
            {
                return NotFound();
            }
            return gallery;
            //return Ok(new { gallery });
        }
*/
        /// <summary>
        /// Create a new gallery.
        /// </summary>
        /// <param name="galleryItem"></param>
        /// <returns></returns>
        // POST: api/Galleries
        [HttpPost]
        public async Task<ActionResult<MediaContainer>> PostGallery(Gallery galleryItem)
        {
            var owner = await _dbContext.Owners.Where(a => a.ExternalUserId == galleryItem.UserId).ToListAsync();
            //get rid of next code is a security risk, we need to inject an additional registration page on signup to collect the owner
            // details. But for now just create an owner record if one does not exist.
            if (owner.Count() == 0)
            {
                _ownerService.AddOwner("", "Auth0", galleryItem.UserId, "", "");
                /*RedirectToAction("PostOwner", "OwnerController", new Owner()
                {
                    EmailAddress = "Unknown",
                    ExternalIdentityProvider = "Auth0",
                    ExternalUserId = galleryItem.UserId,
                    FirstName = "Unknown",
                    LastName = "Unknown"
                });*/
                  
                owner = await _dbContext.Owners.Where(a => a.ExternalUserId == galleryItem.UserId).ToListAsync();
            }

            _dbContext.MediaContainers.Add(new MediaContainer() {
                CreatedDate = galleryItem.CreatedDate,
                Name = galleryItem.Name,
                Description = galleryItem.Description,
                Owner = owner.First()
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
        public async Task<IActionResult> PutGallery(int id, MediaContainer gallery)
        {
            if (id != gallery.ID)
            {
                return BadRequest();
            }

            _dbContext.Entry(gallery).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a gallery for a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Galleries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MediaContainer>> DeleteGallery(int id)
        {
            var gallery = await _dbContext.MediaContainers.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            _dbContext.MediaContainers.Remove(gallery);
            await _dbContext.SaveChangesAsync();
            return gallery;
        }
    }
}
