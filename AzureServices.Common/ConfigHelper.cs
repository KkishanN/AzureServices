using Microsoft.Extensions.Configuration;
using System;

namespace AzureServices.Common
{
    public class ConfigHelper
    {
        public static string CosmosEndpoint { get; set; }
        public static string CosmosPrimaryKey { get; set; }
        public static string SBConnectionString { get; set; }
        public static string QueueName { get; set; }
        public static string RedisCacheConnectionString { get; set; }

        public static void Init(IConfiguration config)
        {
            CosmosEndpoint = "";
            CosmosPrimaryKey = "";
            SBConnectionString = "";
            QueueName = "";
            RedisCacheConnectionString = "";
        }

    }
}