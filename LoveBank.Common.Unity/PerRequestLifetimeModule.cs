using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Web;

namespace LoveBank.Common.Unity
{
    public class PerRequestLifetimeModule : IHttpModule, IDisposable
    {
        private static readonly object key = typeof(PerRequestLifetimeModule).FullName;
        private bool disposed;

        internal static IDictionary<PerRequestLifetimeMananger, object> GetPerRequestLifetimeManagers()
        {
            IDictionary backingStore = BackingStore.Get();
            IDictionary<PerRequestLifetimeMananger, object> instances;

            if (backingStore.Contains(key))
            {
                instances = backingStore[key] as IDictionary<PerRequestLifetimeMananger, object>;
            }
            else
            {
                lock (backingStore)
                {
                    instances = backingStore.Contains(key) ?
                        backingStore[key] as IDictionary<PerRequestLifetimeMananger, object> :
                        new Dictionary<PerRequestLifetimeMananger, object>();

                    if (!backingStore.Contains(key))
                    {
                        if (instances != null) backingStore.Add(key, instances);
                    }
                }
            }

            return instances;
        }

        private static void RemoveAllInstances()
        {
            IDictionary<PerRequestLifetimeMananger, object> lifetimeManagers = GetPerRequestLifetimeManagers();

            var managers = new PerRequestLifetimeMananger[lifetimeManagers.Count];

            lifetimeManagers.Keys.CopyTo(managers, 0);

            managers.Each(lifetimeManager => lifetimeManager.RemoveValue());

            lifetimeManagers.Clear();
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) => RemoveAllInstances();
        }

        [DebuggerStepThrough]
        ~PerRequestLifetimeModule()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                RemoveAllInstances();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
