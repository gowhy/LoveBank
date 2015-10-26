
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
namespace LoveBank.Common.Plugins {

    public abstract class PluginService<T>:IPluginsService<T>{

        private const string DefaultPath = @"Plugins";
        private const string DefaultPluginsName = "LoveBank.Plugins.dll";

        public virtual Dictionary<string, T> ReadPlugins() {
            var approot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPath);
            var ass = Assembly.LoadFile(Path.Combine(approot, DefaultPluginsName));
            var types = ass.GetTypes().Where(o=>o.IsClass && o.GetInterfaces().Any(x=>x.Name == typeof(T).Name));
            var result = new Dictionary<string, T>();
            foreach(var t in types) {
                var o = Activator.CreateInstance(t);
                result.Add(t.Name,(T)Activator.CreateInstance(t));
            }
            return result;
        }

        public virtual  IList<T>  LoadPlugins() {
            var approot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPath);
            var ass = Assembly.LoadFile(Path.Combine(approot, DefaultPluginsName));
            Func<Type, bool> filter = type => type.BaseType != null && (type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType ) &&
                                                                        (type.GetConstructor(Type.EmptyTypes) != null&&type.GetInterfaces().Any(x=>x.Name==typeof(T).Name));
            var types = ass.GetTypes().Where(filter);
            var result = new List<T>();
            foreach (var t in types)
            {
                var o = Activator.CreateInstance(t);
                result.Add((T)Activator.CreateInstance(t));
            }
            return result;
        }

        public virtual T GetPlugins(string key, params dynamic[] para)
        {
            var approot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultPath);
            var ass = Assembly.LoadFile(Path.Combine(approot, DefaultPluginsName));
            var type = ass.GetTypes().FirstOrDefault(o => o.IsClass && o.Name.Equals(key));
            if (type == null)
            {
                throw new Exception("指定接口不存在");
            }
            return (T)Activator.CreateInstance(type,para);
        }

        public abstract void InstallPlugins(string key);
        public abstract void UninstallPlugins(string key);
    }
}