using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public interface IImageStore
    {
        List<BlobItem> GetImages(string container, List<string> fileFilter);
        List<BlobItem> GetImages(string container);
        List<string> GetImageUris(string container);
        Task<string> SaveImage(Stream imageStream, string containerName);
        string UriFor(string container, string item);
    }
}