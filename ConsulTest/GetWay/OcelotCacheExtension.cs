using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GateWay
{
    /// <summary>
    /// 自定义网关缓存
    /// </summary>
    public class OcelotCacheExtension : IOcelotCache<CachedResponse>
    {
        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"-------------------- Add Cache --------------------");
            RedisHelper.Instance.Set(key, value, ttl);
        }

        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"-------------------- AddAndDelete Cache --------------------");
            RedisHelper.Instance.Remove(key);
            RedisHelper.Instance.Set(key, value, ttl);
        }

        public void ClearRegion(string region)
        {
            Console.WriteLine($"-------------------- ClearRegion Cache --------------------");
            throw new NotImplementedException();
        }

        public CachedResponse Get(string key, string region)
        {
            Console.WriteLine($"-------------------- Get Cache --------------------");
            return RedisHelper.Instance.Get<CachedResponse>(key);
        }
    }
}
