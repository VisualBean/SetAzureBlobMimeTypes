using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace SetBlobMimeTypes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "SetBlobMimeTypes";
            app.HelpOption("-?|-h|--help");
            var accountOption = app.Option("-a|--account", "The storage account name.", CommandOptionType.SingleValue);
            var keyOption = app.Option("-k|--key", "The storage account key.", CommandOptionType.SingleValue);
            var containerOption = app.Option("-c|--container", "The initial container.", CommandOptionType.SingleValue);

            app.OnExecute(async () =>
             {
                if (!(accountOption.HasValue() && keyOption.HasValue() && containerOption.HasValue()))
                {
                    Console.WriteLine("Please provide values for all options. (-a -k -c)");
                    return -1;
                }

                CloudBlobContainer container;
                CloudBlobClient client;
                try
                {
                    var storageAccount = new CloudStorageAccount(
                        new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                            accountOption.Value(),
                            keyOption.Value())
                            , true);
                    client = storageAccount.CreateCloudBlobClient();
                    container = client.GetContainerReference(containerOption.Value());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                     
                await SetBlobMimeTypes(container);
                return 0;
                 
             });

            app.Execute(args);

            Console.ReadKey();
            
        }
        public static async Task SetBlobMimeTypes(CloudBlobContainer container)
        {            
            List<CloudBlockBlob> blobItems = await GetBlobItemsAsync(container);

            foreach (var item in blobItems)
            {
                var itemName = item.Name;
                var itemMimeType = MimeTypes.GetMimeType(itemName);
                item.Properties.ContentType = itemMimeType;
                try
                {
                    Console.WriteLine($"Setting mimetype:{itemMimeType} for blob:{itemName}");
                    await item.SetPropertiesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong trying to change mimetype of {itemName}: {ex.Message}");
                    throw;
                }
            }

            Console.WriteLine("Done!");

        }
        private static async Task<List<CloudBlockBlob>> GetBlobItemsAsync(CloudBlobContainer container, string prefix = null)
        {
            var blobItems = new List<CloudBlockBlob>();
            BlobContinuationToken token = null;
            do
            {
                var segment = await container.ListBlobsSegmentedAsync(prefix, token);
                foreach(var item in segment.Results)
                {
                    if (item is CloudBlockBlob blob)
                    {
                        blobItems.Add(blob);
                    }
                    if (item is CloudBlobDirectory dir)
                    {
                        blobItems.AddRange(await GetBlobItemsAsync(container, dir.Prefix));
                    }
                }

                token = segment.ContinuationToken;
            }
            while (token != null);

            return blobItems;
        }
    }
}
