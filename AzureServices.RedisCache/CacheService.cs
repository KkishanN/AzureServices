using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServices.RedisCache
{
    public class CacheService
    {
        private static IDatabase RedisCache { get; }
        static CacheService()
        {
            CacheConnection cc = new CacheConnection();
            RedisCache = cc.GetRedisDatabase(null);
        }

        #region GetCache
        /// <summary>
        /// This function gets cache if exists
        /// </summary>
        /// <param name="key">Cache Key</param>
        public static string GetCache(string key)
        {
            try
            {
                if (RedisCache.KeyExists(key))
                {
                    return RedisCache.StringGet(key);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"GetCache: {e.Message}", e);
            }
        }
        #endregion

        #region SetCache
        /// <summary>
        /// This function sets cache and updates cache if it already exists
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">string to be cached</param>
        /// <param name="cacheExpiry">Cache expiry</param>
        public static void SetCache(string key, string value, double cacheExpiry)
        {
            try
            {
                RedisCache.StringSet(key, value.ToString(), TimeSpan.FromMinutes(cacheExpiry));
            }
            catch (Exception e)
            {
                throw new Exception($"SetCache: {e.Message}", e);
            }
        }
        #endregion

        #region RemoveCache
        /// <summary>
        /// This function removes cache if it exists
        /// </summary>
        /// <param name="key">Cache Key</param>
        public static void RemoveCache(string key)
        {
            try
            {
                if (RedisCache.KeyExists(key))
                    RedisCache.KeyDelete(key);
            }
            catch (Exception e)
            {
                throw new Exception($"RemoveCache: {e.Message}", e);
            }
        }
        #endregion

        #region POCs
        public static void CacheFunctionsPOC()
        {
            try
            {
                Console.WriteLine();
                RedisCache.HashIncrement("hash1", "valu1");
                Console.WriteLine();
                var data = RedisCache.HashGet("hash1", "valu1");
                //RedisCache.HashSet("hashsetkey1", )
                CacheFunctionsPOC();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
