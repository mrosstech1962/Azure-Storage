using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

// Blob naming rules
// https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/

namespace Queues
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnection =
                System.Configuration.ConfigurationManager.AppSettings.Get("StorageConnectionString");

            //** grabs the storage account name from the storage access key an app.config
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);

            // Queues are used for messaging. Notifications are sent to queues, and other apps
            // pickup and process these notifications/messages
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue myqueue = queueClient.GetQueueReference("thisisaqueue");
            myqueue.CreateIfNotExists();

            CloudQueueMessage newmessage = new CloudQueueMessage("This is the fourth message!");
            myqueue.AddMessage(newmessage);

            CloudQueueMessage oldmessage = myqueue.GetMessage();
            Console.WriteLine(oldmessage.AsString);

            Console.ReadKey();
        }
    }
}
