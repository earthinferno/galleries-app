using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public class ImageStore : IImageStore
    {
        string _baseUri;
        IConfiguration _configuration;


        private readonly IMediaStorage _mediaStorage;

        public ImageStore(IMediaStorage mediaStorage, IConfiguration configuration)
        {
            _mediaStorage = mediaStorage;
            _configuration = configuration;
            _baseUri = _configuration["Azure_Blob_BaseUri"];
        }

        /// <summary>
        /// Save an imagestream to the blob store
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task<string> SaveImage(Stream imageStream, string containerName)
        {
            var imageId = Guid.NewGuid().ToString();
            await _mediaStorage.UploadAsync(containerName, imageId, imageStream);
            return imageId;
        }

        public string UriFor(string container, string item)
        {
            return _mediaStorage.ItemUri(_baseUri, container, item).Result;
        }

        public List<string> GetImageUris(string container)
        {
            return _mediaStorage.GetAllMediaUris(_baseUri, container).Result;
        }

        public List<BlobItem> GetImages(string container)
        {
            return _mediaStorage.GetMediaItems(_baseUri, container).Result;
        }

        public List<BlobItem> GetImages(string container, List<MediaItem> mediaItems)
        {
            return _mediaStorage.GetMediaItems(_baseUri, container, mediaItems).Result;
        }

        public void DeleteImage(string containerName, string blobName)
        {
            _mediaStorage.DeleteBlockBlobAsync(containerName, blobName);
        }

    }
}
