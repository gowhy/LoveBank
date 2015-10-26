using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;

namespace LoveBank.Common.Plugins.Sms {
    public class SMSPlugins {
        private const string TypeCacheKey = "SMSPlugins_Cache";
        private static readonly Cache pluginCache = HttpRuntime.Cache;


        private static volatile SMSPlugins instance;
        private static readonly object LockHelper = new object();

        private SMSPlugins() {
        }

        private string PluginLocalPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins"); } }

        private string PluginFilePath { get { return Path.Combine(PluginLocalPath, "LoveBank.Plugins.dll"); } }

        public IEnumerable<Type> GetPlugins() {
            Func<Type, bool> filter = type => type.BaseType != null && (type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType) &&
                                                                        type.GetInterfaces().Any(x => x.Name == typeof(ISmsSender).Name);
            var types=Assembly.LoadFile(PluginFilePath).GetTypes().Where(filter).ToList();

            return types;
        }

        public Type GetPlugin(string name) {

            var cache = GetPluginCache();

            name = name.ToLower();

            var type = cache[name] as Type;

            if (type == null) {
                type = Assembly.LoadFile(PluginFilePath).GetType(name, false, true);
                if (type != null)
                    cache[name] = type;
            }


            return type;
        }
        
        public static SMSPlugins Instance() {
            if (instance == null) {
                lock (LockHelper) {
                    if (instance == null) {
                        instance = new SMSPlugins();
                    }
                }
            }
            return instance;
        }

        private Hashtable GetPluginCache() {
            var hashtable = pluginCache.Get(TypeCacheKey) as Hashtable;

            if (hashtable == null) {
                hashtable = new Hashtable();
                pluginCache.Insert(TypeCacheKey, hashtable, new CacheDependency(PluginLocalPath));
            }
            return hashtable;
        }
    }
}