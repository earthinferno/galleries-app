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


        private readonly IMediaStorage _imageStorage;

        public ImageStore(IMediaStorage mediaStorage, IConfiguration configuration)
        {
            _imageStorage = mediaStorage;
            _configuration = configuration;
            _baseUri = _configuration["Azure_Blob_BaseUri"];
        }
        public async Task<string> SaveImage(Stream imageStream, string containerName)
        {
            var imageId = Guid.NewGuid().ToString();
            await _imageStorage.UploadAsync(containerName, imageId, imageStream);
            return imageId;
        }

        public string UriFor(string container, string item)
        {
            return _imageStorage.ItemUri(_baseUri, container, item).Result;
        }

        public List<string> GetImageUris(string container)
        {
            return _imageStorage.GetAllMediaUris(_baseUri, container).Result;
        }

        public List<BlobItem> GetImages(string container)
        {
            return _imageStorage.GetMediaItems(_baseUri, container).Result;
        }

        public List<BlobItem> GetImages(string container, List<string> fileFilter)
        {
            return _imageStorage.GetMediaItems(_baseUri, container, fileFilter).Result;
        }

    }
}
