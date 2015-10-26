using System.Web.Caching;

namespace LoveBank.MVC
{
    public interface ICacheProvider
    {
        object Get(string key);
        void Remove(string key);
        void Insert(string key, object value);
        void Insert(string key, object value, CacheDependency dependency);
        void Insert(string key, object value, CacheItemRemovedCallback cacheItemRemovedCallback, params string[] fileDependencies);
    }
}
