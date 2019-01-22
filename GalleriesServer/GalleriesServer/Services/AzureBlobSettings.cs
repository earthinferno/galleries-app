namespace GalleriesServer.Services
{
    public class AzureBlobSettings
    {
        public AzureBlobSettings(string storageAccount, string storageKey)
        {
            Guard.GuardStringValue(storageAccount, "StorageAccount");
            Guard.GuardStringValue(storageKey, "StorageKey");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
        }

        public string StorageAccount { get; }
        public string StorageKey { get; }
    }
}
