﻿namespace GalleriesServer.Services
{
    public class AzureBlobSettings
    {
        public AzureBlobSettings(string storageAccount, string storageKey, string containerName)
        {
            Guard.GuardStringValue(storageAccount, "StorageAccount");
            Guard.GuardStringValue(storageKey, "StorageKey");
            Guard.GuardStringValue(containerName, "containerName");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            ContainerName = containerName;
        }

        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string ContainerName { get; }
    }
}
