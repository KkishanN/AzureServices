using AzureServices.Common;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;

namespace AzureServices.RedisCache
{
    public class CacheConnection
    {
        public static IDatabase RedisCache { get; set; }
        public static IMemoryCache Cache { get; set; }

        #region Cache Init
        /// <summary>
        /// This function initializes cache object
        /// </summary>
        /// <param name="cache">IMemoryCache object</param>
        public static void Init(IMemoryCache cache)
        {
            try
            {
                Cache = cache;
                if (RedisCache == null)
                    RedisCache = lazyConnection.Value.GetDatabase();
            }
            catch (Exception e)
            {
                throw new Exception($" Init: {e.Message}", e);
            }
        }
        #endregion

        #region Redis Cache Lazy Connection
        /// <summary>
        /// This function initializes connection with Redis Cache
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ConfigHelper.RedisCacheConnectionString));

        public IDatabase GetRedisDatabase(IMemoryCache cache)
        {
            Init(cache);
            return RedisCache;
        }

        #endregion
    }
}
