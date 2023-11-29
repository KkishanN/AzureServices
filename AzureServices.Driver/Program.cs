using AzureServices.RedisCache;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.Driver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> langs = new List<string>() { "en", "ja", "es", "hi", "fr" };
            List<string> langCodePairs = new List<string>();
            for (int i = 0; i < langs.Count; i++)
                for (int j = 0; j < langs.Count; j++)
                {
                    if (i != j)
                    {
                        langCodePairs.Add($"\"{langs[i]}-{langs[j]}\"");
                    }
                }
            string langCodePairsStringify = string.Join(", ", langCodePairs);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
