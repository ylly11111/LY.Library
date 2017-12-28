using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.CacheManager
{
   public   class CacheManagerClass
    {
       public static ICacheManager<Object> Cache;
       static CacheManagerClass()
       {
           Cache = CacheFactory.Build("CacheInstance", setting =>
               {
                   setting.WithSystemRuntimeCacheHandle("ly");
               });
       }

       public static void Add(string key,object obj)
       {
           Cache.Add(key, obj);
       }

       public static void Put(string key,object obj)
       {
           Cache.Put(key, obj);
       
       }

       public static T Get<T>(string key)
       {
           return Cache.Get<T>(key);
       }
    }
}
