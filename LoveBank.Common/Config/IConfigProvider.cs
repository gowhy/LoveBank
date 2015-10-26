using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Config
{
    public interface IConfigProvider {
        /// <summary>
        /// 获得配置文件
        /// </summary>
        /// <param name="key">配置文件的key</param>
        /// <param name="configType">配置文件的类型</param>
        /// <returns></returns>
        IConfig Get(string key,Type configType);

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="key">配置文件的key</param>
        /// <param name="config">配置文件实例</param>
        bool Save(string key,IConfig config);

    }
}
