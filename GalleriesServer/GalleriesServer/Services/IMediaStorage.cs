using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public interface IMediaStorage
    {
        Task UploadAsync(string containerName, string itemName, Stream stream);
        Task<string> ItemUri(string baseUri, string containerName, string itemName);
        Task<List<string>> GetAllMediaUris(string baseUri, string containerName);
        Task<List<BlobItem>> GetMediaItems(string baseUri, string containerName);
    }
}