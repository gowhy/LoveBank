using System;
using System.Collections.Generic;

namespace LoveBank.Common
{
    public interface IServiceResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object Resolve(Type serviceType, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object Resolve(Type serviceType);
        
        /// <summary>
        /// Resolve All instance from type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type serviceType);
    }
}
