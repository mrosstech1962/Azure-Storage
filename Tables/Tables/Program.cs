using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Tables
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnection =
                System.Configuration.ConfigurationManager.AppSettings.Get("StorageConnectionString");

            //** grabs the storage account name from the storage access key an app.config
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Creates the NoSQL table in the storage account
            CloudTable table = tableClient.GetTableReference("FirstTable");
            table.CreateIfNotExists();

            //CarEntity newcar = new CarEntity(260, 2011, "AMC", "Brad", "Red");
            //TableOperation insert = TableOperation.Insert(newcar);
            //table.Execute(insert);

            // The next 9 rows shows how three more items are added to the table
            // The ID, or the first integer in the array must be a unique ID or it will fail.
            // newcar = new CarEntity(261, 2012, "Ford", "CRV", "Yellow");
            // insert = TableOperation.Insert(newcar);
            //table.Execute(insert);
            // newcar = new CarEntity(262, 2013, "JuJu", "Flash", "White");
            // insert = TableOperation.Insert(newcar);
            //table.Execute(insert);
            // newcar = new CarEntity(263, 2014, "Jaguar", "Expensive", "Black");
            // insert = TableOperation.Insert(newcar);
            //table.Execute(insert);

            // Entity Group Transaction
            // Table batch operations can be done 100 at a time.
            // They must all be using the same partion key "car"
            TableBatchOperation tbo = new TableBatchOperation();
            CarEntity newcar = new CarEntity(274, 2012, "BMW", "X1", "Black");
            tbo.Insert(newcar);
            newcar = new CarEntity(275, 2012, "Honda", "Civic", "Yellow");
            tbo.Insert(newcar);
            newcar = new CarEntity(276, 2013, "BMW", "X1", "White");
            tbo.Insert(newcar);
            newcar = new CarEntity(277, 2014, "BMW", "X1", "Silver");
            tbo.Insert(newcar);
            table.ExecuteBatch(tbo);

            // The next 11 lines show how a simple row is retrieved.
            //TableOperation retrieve = TableOperation.Retrieve<CarEntity>("car", "123");
            //TableResult result = table.Execute(retrieve);

            //if (result.Result == null)
            //{
            //    Console.WriteLine("not found");
            //}
            //else
            //{
            //    Console.WriteLine("Car Found: " + ((CarEntity)result.Result).Make + " " + ((CarEntity)result.Result).Model);
            //}

            // This query returns all rows
            TableQuery<CarEntity> carquery = new TableQuery<CarEntity>();
            // This query returns the first two rows from the table
            //TableQuery<CarEntity> carquery = new TableQuery<CarEntity>().Take(2);
            foreach (CarEntity thiscar in table.ExecuteQuery(carquery))
            {
                Console.WriteLine(thiscar.Year + " | " + thiscar.Make + " | " + thiscar.Model + " | " + thiscar.Color + "\n");
            }

            Console.ReadKey();
        }
    }

    public class CarEntity : TableEntity
    {
        public CarEntity(int ID, int year, string make, string model, string color)
        {
            this.UniqueID = ID;
            this.Year = year;
            this.Make = make;
            this.Model = model;
            this.Color = color;
            this.PartitionKey = "car";
            this.RowKey = ID.ToString();
        }

        public CarEntity() { }

        public int UniqueID { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

    }
}
