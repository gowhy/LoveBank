using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Plugins
{
    public class PaymentManager:PluginService<IPayment>
    {
        #region Overrides of PluginService<IPayment>

        /// <summary>
        /// 安装插件
        /// </summary>
        /// <param name="key"/>
        public override void InstallPlugins(string key) {
            throw new NotImplementedException();
        }

        public override void UninstallPlugins(string key) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
