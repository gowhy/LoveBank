using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace LoveBank.Common
{
    public interface IContainerAdapter : IServiceRegister, IServiceResolver, IServiceInjector, IDisposable
    {
    }

    public abstract class ContainerAdapter : IContainerAdapter
    {
        #region Implement IServiceRegister

        public abstract IServiceRegister RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime);

        public abstract IServiceRegister RegisterType<TService>(string key, Func<IServiceResolver, TService> constructFactory, LifetimeType lifetime);

        public abstract IServiceRegister RegisterInstance(string key, Type serviceType, object instance);

        #endregion

        #region Implement IServiceResolver

        public virtual object Resolve(Type serviceType, string key)
        {
            try
            {
                return DoGetService(serviceType, key);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public virtual object Resolve(Type serviceType)
        {
            return Resolve(serviceType, null);
        }

        public virtual IEnumerable<object> ResolveAll(Type serviceType)
        {
            try
            {
                return DoGetServices(serviceType);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }

        protected abstract object DoGetService(Type serviceType, string key);

        protected abstract IEnumerable<object> DoGetServices(Type serviceType);

        #endregion

        #region Implement IServiceInjector

        public abstract void Inject(object instance);

        #endregion

        #region IDisposable

        private bool disposed;

        ~ContainerAdapter()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        protected virtual void DisposeCore()
        {

        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                DisposeCore();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
