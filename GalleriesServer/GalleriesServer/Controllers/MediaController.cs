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
        private readonly IImageStore _imageStore;
        private readonly OwnerService _ownerService;
        private readonly MediaContainerService _containerService;
        private readonly MediaItemService _itemService;


        public MediaController(
            GalleriesDbContext dbContext,
            IImageStore imageStore,
            OwnerService ownerService, 
            MediaContainerService containerService,
            MediaItemService itemService)
        {
            _dbContext = dbContext; // as GalleriesDbContext;
            _imageStore = imageStore;
            _ownerService = ownerService;
            _containerService = containerService;
            _itemService = itemService;
        }

        /*
        [HttpGet("{galleryId}")]
        public async Task<ActionResult<IEnumerable<MediaItem>>> GetItems(int galleryId)
        {
            var gallery = await _dbContext.MediaContainers.FindAsync(galleryId);
            return Ok(gallery.MediaItems as List<MediaItem>);
        }
        */

        /// <summary>
        /// Upload a file to the server blob store. Additionally save metadata to the repository.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MediaItem>> Upload(IFormCollection form)
        {

            var userFolder = form["UserFolder"];
            var comment = form["Comment"];
            var userId = form["UserId"];

            var filepath = "";
            var blobName = "";

            if (string.IsNullOrEmpty(userFolder) || string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            // return bad request if the owener does not exist.
            var owner = await _ownerService.GetOwner(userId);
            if (owner == null)
            {
                return BadRequest();
            }

            // Return badrequest if the gallery doesn't exist.
            var mediaContainer = await _containerService.GetMediaContainerByName(owner, userFolder);
            if (mediaContainer == null)
            {
                return BadRequest();
            }

            // Return badrequest if all upload files have 0 size.
            if (form.Files.Count > 0 && form.Files.Where(a => a.Length > 0).Count() == 0)
            {
                return BadRequest();
            }

            foreach (var file in form.Files)
            {
                try
                {
                    if (file.Length > 0)
                    {
                        // Save the file as a blob
                        filepath = Path.GetTempPath() + file.FileName;
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            blobName = await _imageStore.SaveImage(stream, userFolder);
                        }

                        // Create the metadata about the added media item.
                        _itemService.AddMediaItem(mediaContainer, new MediaItem() { Comment = comment, FileName = file.FileName, ImageUri = blobName });
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }

            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetItems", new { galleryId = mediaContainer.ID, owner = mediaContainer.Owner.ID}, mediaContainer.MediaItems);
        }

        /// <summary>
        /// Update existing meta data about a file.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
            // PUT: api/media/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMedia(int id, MediaItem item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }

            try
            {
                await _itemService.UpdateMediaItem(item);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }

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
            try
            {
                await _itemService.DeleteMediaItem(id);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }


            //TODO: delete the blob as well. Don't leave it on the server.

            return NoContent();
        }
    }
}
