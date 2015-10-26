using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LoveBank.Common;


namespace LoveBank.MVC
{
    public class ExtendDependencyResolver : IDependencyResolver
    {
        private readonly IServiceResolver _resolver;

        public ExtendDependencyResolver(IServiceResolver resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return _resolver.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.ResolveAll(serviceType);
        }
    }
}