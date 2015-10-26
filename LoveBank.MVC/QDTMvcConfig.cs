using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LoveBank.Common;
using LoveBank.MVC.Security;
using LoveBank.MVC.SiteMap;

namespace LoveBank.MVC
{
    public class LoveBankMvcConfig
    {
        private static readonly LoveBankMvcConfig Instance = new LoveBankMvcConfig();

        private  IMvcRegister _mvcRegister = new DefaultRegister();

        private Action<SiteMapDictionary> _sitemapRegister;

        private Action<SecurityManager> _securityRegister;

        private LoveBankMvcConfig()
        {
        }

        public static void Configure(Action<LoveBankMvcConfig> action)
        {
            Check.Argument.IsNotNull(action, "action");
            action(Instance);

            Instance.Init();
        }

        private void Init()
        {
            _mvcRegister.Register();

            //_sitemapRegister(SiteMapManager.SiteMaps);

            SecurityManager.Configure(_securityRegister);
        }

        public LoveBankMvcConfig Register(IMvcRegister register)
        {
            _mvcRegister = register;
            return this;
        }

        public LoveBankMvcConfig SiteMap(Action<SiteMapDictionary> maps)
        {
            _sitemapRegister = maps;
            return this;
        }

        public LoveBankMvcConfig Sercurity(Assembly assembly,Action<SecurityManager> action)
        {
            Action<SecurityManager> proxy = x =>
                                                {
                                                    x.LoadAssembly(assembly);
                                                    action(x);
                                                };
            _securityRegister = proxy;

            return this;
        }
    }
}
