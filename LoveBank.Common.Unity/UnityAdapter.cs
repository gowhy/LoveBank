using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace LoveBank.Common.Unity
{
    public class UnityAdapter:ContainerAdapter
    {
        public UnityAdapter(IUnityContainer container)
        {
            Check.Argument.IsNotNull(container, "container");

            Container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IUnityContainer Container
        {
            get;
            private set;
        }

        public override IServiceRegister RegisterType(string key, Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            Check.Argument.IsNotNull(serviceType, "serviceType");
            Check.Argument.IsNotNull(implementationType, "implementationType");

            LifetimeManager lifetimeManager = (lifetime == LifetimeType.PerRequest) ?
                                              new PerRequestLifetimeMananger() :
                                              ((lifetime == LifetimeType.Singleton) ?
                                              new ContainerControlledLifetimeManager() :
                                              new TransientLifetimeManager() as LifetimeManager);

            if (string.IsNullOrEmpty(key))
            {
                if (Container.Registrations.Any(registration => registration.RegisteredType == serviceType))
                {
                    Container.RegisterType(serviceType, implementationType, implementationType.FullName, lifetimeManager);
                }
                else
                {
                    Container.RegisterType(serviceType, implementationType, lifetimeManager);
                }
            }
            else
            {
                Container.RegisterType(serviceType, implementationType, key, lifetimeManager);
            }

            return this;
        }

        public override IServiceRegister RegisterType<TService>(string key, Func<IServiceResolver, TService> constructFactory, LifetimeType lifetime)
        {
            LifetimeManager lifetimeManager = (lifetime == LifetimeType.PerRequest) ?
                                              new PerRequestLifetimeMananger() :
                                              ((lifetime == LifetimeType.Singleton) ?
                                              new ContainerControlledLifetimeManager() :
                                              new TransientLifetimeManager() as LifetimeManager);

            if (string.IsNullOrEmpty(key))
            {
                if (Container.Registrations.Any(registration => registration.RegisteredType == typeof(TService)))
                {
                    Container.RegisterType<TService>(typeof(TService).FullName, lifetimeManager, new InjectionFactory(x => constructFactory.Invoke(this)));
                }
                else
                {
                    Container.RegisterType<TService>(lifetimeManager, new InjectionFactory(x => constructFactory.Invoke(this)));
                }
            }
            else
            {
                Container.RegisterType<TService>(key, lifetimeManager, new InjectionFactory(x => constructFactory.Invoke(this)));
            }
            return this;
        }

        public override IServiceRegister RegisterInstance(string key, Type serviceType, object instance)
        {
            Check.Argument.IsNotNull(serviceType, "serviceType");
            Check.Argument.IsNotNull(instance, "instance");

            if (string.IsNullOrEmpty(key))
            {
                Container.RegisterInstance(serviceType, instance);
            }
            else
            {
                Container.RegisterInstance(serviceType, key, instance);
            }

            return this;
        }

        public override void Inject(object instance)
        {
            if (instance != null)
            {
                Container.BuildUp(instance.GetType(), instance);
            }
        }

        protected override object DoGetService(Type serviceType, string key)
        {
            try
            {
                return string.IsNullOrWhiteSpace(key)?Container.Resolve(serviceType):Container.Resolve(serviceType, key);
            }catch(ResolutionFailedException)
            {
                return null;
            }
        }

        protected override IEnumerable<object> DoGetServices(Type serviceType)
        {
            Check.Argument.IsNotNull(serviceType, "serviceType");

            var instances = new List<object>();

            if (Container.Registrations.Any(registration => registration.RegisteredType == serviceType && string.IsNullOrEmpty(registration.Name)))
            {
                instances.Add(Container.Resolve(serviceType));
            }

            instances.AddRange(Container.ResolveAll(serviceType));

            return instances;
        }

        protected override void DisposeCore()
        {
            Container.Dispose();
            base.DisposeCore();
        }
    }
}
