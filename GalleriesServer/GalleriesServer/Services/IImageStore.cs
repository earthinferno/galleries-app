using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public interface IImageStore
    {
        /// <summary>
        /// Get all stored images from container associated with a list of media items.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fileFilter"></param>
        /// <returns></returns>
        List<BlobItem> GetImages(string container, List<MediaItem> mediaItems);


        //List<BlobItem> GetImages(string container);


        //List<string> GetImageUris(string container);

        /// <summary>
        /// Save an image to the blob store
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        Task<string> SaveImage(Stream imageStream, string containerName);


        //string UriFor(string container, string item);
        
        
        void  DeleteImage(string containerName, string blobName);
    }
}