﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleriesServer.Models;

namespace GalleriesServer.Services
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly AzureBlobSettings _blobSettings;
        public AzureBlobStorage(CloudStorageAccount StorageAccount, AzureBlobSettings blobSettings)
        {
            _storageAccount = StorageAccount;
            _blobSettings = blobSettings;

        }
        private async Task<CloudBlobContainer> GetContainerAsync()
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(_blobSettings.ContainerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string blobName)
        {
            var container = await GetContainerAsync();
            return container.GetBlockBlobReference(blobName);
        }

        private async Task<List<AzureBlobItem>> GetBlobListAsync(bool useFlat = true)
        {
            var container = await GetContainerAsync();
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

        private async Task<CloudBlockBlob> getBlobREferenceAsync(string imageId)
        {
            var container = await GetContainerAsync();
            return container.GetBlockBlobReference(imageId);
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

        public async Task UploadAsync(string blobName, Stream stream)
        {
            var blob = await GetBlockBlobAsync(blobName);
            await blob.UploadFromStreamAsync(stream);
        }

        //public async Task<MemoryStream> DownloadAsync(string blobName)
        //{
        //    throw new ApplicationException("Please access the storage resource directly.");
        //}

        public async Task<List<AzureBlobItem>> ListAsync()
        {
            return await GetBlobListAsync();
        }

        public async Task<List<string>> ListFolderAsync()
        {
            var list = await GetBlobListAsync();
            return list.Where(i => !string.IsNullOrEmpty(i.Folder))
                .Select(i => i.Folder)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
        }


        public async Task<string> BlobUri(string baseUri, string blobName)
        {
            SharedAccessBlobPolicy sasPolicy = getSAS();

            var blob = await getBlobREferenceAsync(blobName);
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseUri}images/{blobName}{sas}";
        }

        public async Task<List<string>> GetBlobUris(string baseUri)
        {
            var sasPolicy = getSAS();
            var list = await GetBlobListAsync();
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => $"{baseUri}images/{i.BlobName}{i.BlockBlobSharedAccessSignature}")
                .Distinct()
                .OrderBy(i => i)
                .ToList();

        }

        public async Task<List<string>> GetBlobImageId()
        {
            var list = await GetBlobListAsync();
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => i.BlobName)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
        }

        public async Task<List<BlobItem>> GetImageBlobs(string baseUri)
        {
            var list = await GetBlobListAsync();
            return list.Where(i => !string.IsNullOrEmpty(i.BlobName) && i.IsBlockBlob)
                .Select(i => new BlobItem() { Uri = $"{baseUri}images/{i.BlobName}{i.BlockBlobSharedAccessSignature}", BlobName = i.BlobName })
                .Distinct()
                //.OrderBy(i => i.Uri)
                .ToList();
        }


    }
}
