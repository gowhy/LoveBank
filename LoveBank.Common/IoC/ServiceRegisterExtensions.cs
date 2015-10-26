using System;
using System.Diagnostics;

namespace LoveBank.Common
{
    public static class ServiceRegisterExtensions
    {

        /// <summary>
        /// Registers the instance as a singleton service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterInstance<TService>(this IServiceRegister instance, object service)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterInstance(null, typeof(TService), service);
        }

        /// <summary>
        /// Registers the instance as a singleton service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">key</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static IServiceRegister RegisterInstance<TService>(this IServiceRegister instance,string key,object service)
        {
            Check.Argument.IsNotEmpty(key,"key");
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterInstance(key, typeof (TService), service);
        }

        /// <summary>
        /// Registers the instance as a singleton service of the instance type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterInstance(this IServiceRegister instance, object service)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterInstance(null, service.GetType(), service);
        }

        /// <summary>
        /// Registers the instance as a singleton service of the instance type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterInstance(this IServiceRegister instance,string key,object service)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterInstance(key, service.GetType(), service);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsPerRequest<TImplementation>(this IServiceRegister instance) where TImplementation : class
        {
            return RegisterAsPerRequest<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers as per request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsPerRequest<TService, TImplementation>(this IServiceRegister instance)
            where TImplementation : TService
            where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.PerRequest);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsPerRequest(this IServiceRegister instance, Type implementationType)
        {
            return RegisterAsPerRequest(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsPerRequest(this IServiceRegister instance, Type serviceType, Type implementationType)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterType(null, serviceType, implementationType, LifetimeType.PerRequest);
        }

        /// <summary>
        /// Registers the service as per request.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <param name="constructFactory"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsPerRequest<TService>(this IServiceRegister instance, Func<IServiceResolver, TService> constructFactory)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterType<TService>(null, constructFactory, LifetimeType.PerRequest);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsSingleton<TImplementation>(this IServiceRegister instance) where TImplementation : class
        {
            return RegisterAsSingleton<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers as singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsSingleton<TService, TImplementation>(this IServiceRegister instance)
            where TImplementation : TService
            where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.Singleton);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsSingleton(this IServiceRegister instance, Type implementationType)
        {
            return RegisterAsSingleton(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as singleton.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsSingleton(this IServiceRegister instance, Type serviceType, Type implementationType)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterType(null, serviceType, implementationType, LifetimeType.Singleton);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsTransient<TImplementation>(this IServiceRegister instance) where TImplementation : class
        {
            return RegisterAsTransient<TImplementation, TImplementation>(instance);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsTransient<TService, TImplementation>(this IServiceRegister instance)
            where TImplementation : TService
            where TService : class
        {
            return RegisterType<TService, TImplementation>(instance, LifetimeType.Transient);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsTransient(this IServiceRegister instance, Type implementationType)
        {
            return RegisterAsTransient(instance, implementationType, implementationType);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsTransient(this IServiceRegister instance, Type serviceType, Type implementationType)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterType(null, serviceType, implementationType, LifetimeType.Transient);
        }

        /// <summary>
        /// Registers the service as transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="constructFactory">constract factory</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IServiceRegister RegisterAsTransient<TService>(this IServiceRegister instance, Func<IServiceResolver, TService> constructFactory)
        {
            Check.Argument.IsNotNull(instance, "instance");
            return instance.RegisterType<TService>(null, constructFactory, LifetimeType.Transient);
        }

        private static IServiceRegister RegisterType<TService, TImplementation>(this IServiceRegister instance, LifetimeType lifetime)
            where TImplementation : TService
            where TService : class
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RegisterType(null, typeof(TService), typeof(TImplementation), lifetime);
        }
    }
}
