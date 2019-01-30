using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public class AzureBlobStorage : IMediaStorage
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly AzureBlobSettings _blobSettings;
        public AzureBlobStorage(CloudStorageAccount StorageAccount, AzureBlobSettings blobSettings)
        {
            _storageAccount = StorageAccount;
            _blobSettings = blobSettings;

        }
        private async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string containerName, string blobName)
        {
            var container = await GetContainerAsync(containerName);
            return container.GetBlockBlobReference(blobName);
        }

        private async Task<List<AzureBlobItem>> GetBlobListAsync(string containerName, bool useFlat = true)
        {
            var container = await GetContainerAsync(containerName);
            var list = new List<AzureBlobItem>();
            BlobContinuationToken token = null;

            do
            {
                var resultSegment = await container.ListBlobsSegmentedAsync("", useFlat, new BlobListingDetails(), null, token, null, null);
                token = resultSegment.ContinuationToken;

                foreach (IListBlobItem item in resultSegment.Results)
                {
                    list.Add(new AzureBlobItem(item));
                }
            } while (token != null);
            return list;
        }

        
        private async Task<CloudBlockBlob> getBlobREferenceAsync(string containerName, string blobReference)
        {
            var container = await GetContainerAsync(containerName);
            return container.GetBlockBlobReference(blobReference);
        }
        

        private static SharedAccessBlobPolicy getSAS()
        {
            return new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };
        }

        public async Task UploadAsync(string containerName, string itemName, Stream stream)
        {
            var blob = await GetBlockBlobAsync(containerName, itemName);
            await blob.UploadFromStreamAsync(stream);
        }

        /* public async Task<List<string>> ListFolderAsync(string containerName)
        {
            var list = await GetBlobListAsync(containerName);
            return list.Where(i => !string.IsNullOrEmpty(i.Container))
                .Select(i => i.Container)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
        }*/


        public async Task<string> ItemUri(string baseUri, string containerName, string itemName)
        {
            SharedAccessBlobPolicy sasPolicy = getSAS();

            var blob = await getBlobREferenceAsync(containerName, itemName);
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseUri}images/{itemName}{sas}";
        }

        public async Task<List<string>> GetAllMediaUris(string baseUri, string containerName)
        {
            var sasPolicy = getSAS();
            var list = await GetBlobListAsync(containerName);
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => $"{baseUri}images/{i.BlobName}{i.BlockBlobSharedAccessSignature}")
                .Distinct()
                .OrderBy(i => i)
                .ToList();

        }

        /*
        public async Task<List<string>> GetBlobImageId()
        {
            var list = await GetBlobListAsync();
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => i.BlobName)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
        }
        */

        public async Task<List<BlobItem>> GetMediaItems(string baseUri, string containerName)
        {
            var list = await GetBlobListAsync(containerName);
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => new BlobItem() { Uri = $"{baseUri}images/{i.BlobName}{i.BlockBlobSharedAccessSignature}", BlobName = i.BlobName })
                .Distinct()
                //.OrderBy(i => i.Uri)
                .ToList();
        }

        public async Task<List<BlobItem>> GetMediaItems(string baseUri, string containerName, List<string> fileFilter)
        {
            var list = await GetBlobListAsync(containerName);
            var unfilteredList = list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => new BlobItem() { Uri = $"{baseUri}{containerName}/{i.BlobName}{i.BlockBlobSharedAccessSignature}", BlobName = i.BlobName })
                .Distinct()
                .ToList();
            List<BlobItem> filteredList = new List<BlobItem>();
            foreach (var blobItem in unfilteredList)
            {
                if(fileFilter.Contains(blobItem.BlobName))
                {
                    filteredList.Add(blobItem);
                }
            }
            return filteredList;
        }
    }
}
