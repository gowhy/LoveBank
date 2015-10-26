using System;

namespace LoveBank.Common
{
    public interface IServiceRegister
    {
        /// <summary>
        /// Registers the service and its implementation with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegister RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime);

        /// <summary>
        /// Registers the service by construct factory with the lifetime behavior.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="constructFactory">construct factory</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <returns></returns>
        IServiceRegister RegisterType<TService>(string key, Func<IServiceResolver, TService> constructFactory, LifetimeType lifetime);

        /// <summary>
        /// Registers the instance as singleton.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IServiceRegister RegisterInstance(string key, Type serviceType, object instance);
    }
}
