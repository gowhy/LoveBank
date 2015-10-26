using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common;

namespace LoveBank.MVC
{
    public class DefaultRegister : IMvcRegister
    {
        public void Register()
        {
            IoC.Current
                .RegisterAsSingleton<IPathResolver, PathResolver>()
                .RegisterAsSingleton<IVirtualPathProvider, VirtualPathProviderWrapper>()
                .RegisterAsSingleton<ICacheProvider, WebCacheProvider>();
        }
    }
}
