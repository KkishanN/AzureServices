using AzureServices.Common;
using Microsoft.Azure.Cosmos;
using System;

namespace AzureServices.CosmosDB
{
    public class DatabaseConnection
    {
        public static CosmosClient CosmosDBClient { get; set; }
        public static Database database { get; set; }
        static DatabaseConnection()
        {
            try
            {
                var options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway
                };
                CosmosDBClient = new CosmosClient(ConfigHelper.CosmosEndpoint, ConfigHelper.CosmosPrimaryKey, options);
                database = CosmosDBClient.CreateDatabaseIfNotExistsAsync("speakerdb").Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
