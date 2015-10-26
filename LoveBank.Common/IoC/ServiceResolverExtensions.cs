using System.Collections.Generic;

namespace LoveBank.Common
{
    public static class ServiceResolverExtensions
    {
        public static T Resolve<T>(this IServiceResolver container, string key)
        {
            Check.Argument.IsNotNull(container, "container");

            return (T)container.Resolve(typeof(T), key);
        }

        public static T Resolve<T>(this IServiceResolver container)
        {
            Check.Argument.IsNotNull(container, "container");

            return (T)container.Resolve(typeof(T));
        }

        public static IEnumerable<T> ResolveAll<T>(this IServiceResolver container)
        {
            Check.Argument.IsNotNull(container, "container");

            return (IEnumerable<T>)container.ResolveAll(typeof(T));
        }
    }
}
