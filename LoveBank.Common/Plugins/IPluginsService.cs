
using System.Collections.Generic;

namespace LoveBank.Common.Plugins {
    /// <summary>
    /// 插件服务，用于安装，读取插件
    /// </summary>
    public interface IPluginsService<T>{
        /// <summary>
        /// 读取插件列表
        /// </summary>
        /// <returns></returns>
        Dictionary<string, T> ReadPlugins();

        /// <summary>
        /// 通过类名取得相应插件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="para"> </param>
        /// <returns></returns>
        T GetPlugins(string key, params dynamic[] para);
        /// <summary>
        /// 安装插件
        /// </summary>
        /// <param name="key"></param>
        void InstallPlugins(string key);

        void UninstallPlugins(string key);
    }
}