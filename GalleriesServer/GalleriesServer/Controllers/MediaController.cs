using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;
using GalleriesServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleriesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : Controller
    {
        private readonly GalleriesDbContext _dbContext;
        private readonly OwnerService _ownerService;
        private readonly ImageStore _imageStore;


        public MediaController(GalleriesDbContext dbContext, OwnerService ownerService, ImageStore imageStore)
        {
            _dbContext = dbContext;
            _ownerService = ownerService;
            _imageStore = imageStore;

        }

        /// <summary>
        /// Returns all the media items in a gallery.
        /// </summary>
        /// <param name="galleryId"></param>
        /// <returns></returns>
        // GET: api/media/userId
        [HttpGet("{galleryId}")]
        public async Task<ActionResult<IEnumerable<MediaItem>>> GetItems(int galleryId)
        {
            //var items = await _dbContext.MediaContainers.Where(a => a.ID == galleryId).Select(a => a.MediaItems).ToListAsync();   
            var items = await _dbContext.MediaContainers.Where(a => a.ID == galleryId).Select(a => a.MediaItems).FirstAsync() as List<MediaItem>;
            return items;
        }


        /// <summary>
        /// Upload a file to the server blob store. Additionally save metadata to the repository.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MediaItem>> Upload(IFormCollection form)
        {

            var comment = form["Comment"];

            var filepath = "";
            var blobName = "";
            int galleryId;
            if (int.TryParse(form["GalleryId"], out galleryId))
            {
                foreach (var file in form.Files)
                {
                    if (file.Length > 0)
                    {
                        // Save the file as a blob
                        filepath = Path.GetTempPath() + file.FileName;
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            //await file.CopyToAsync(stream);
                            blobName = await _imageStore.SaveImage(stream);
                        }

                        // save the meta data associated with the blob. 
                        // There will be only 1 file so this return is okay
                        return await SaveMetaData(new GalleryItem
                        {
                            MediaItem = new MediaItem { Comment = comment, FileName = file.Name, ImageUri = blobName },
                            MediaContainerID = galleryId
                        });
                    }
                }
            }
            return BadRequest();
            //return Ok(new { results = images });
        }


        private async Task<ActionResult<MediaItem>> SaveMetaData(GalleryItem item)
        {
            _dbContext.MediaContainers.Where(a => a.ID == item.MediaContainerID).First().MediaItems.Add(item.MediaItem);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetItems", new { galleryId = item.MediaContainerID }, item);
        }

        /// <summary>
        /// Update existing meta data about a file.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
            // PUT: api/media/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedia(int id, MediaItem item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }

            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete the supplied media item from the repository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/media/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MediaItem>> DeleteMedia(int id)
        {
            //var gallery = await _dbContext.MediaContainers.FindAsync(userId);
            var item = await _dbContext.MediaItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();

            //TODO: delete the blob as well. Don't leave it on the server.

            return item;
        }
    }
}
