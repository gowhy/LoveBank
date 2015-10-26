using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using LoveBank.Common;
using LoveBank.Common.Config;

namespace LoveBank.Core.MSData {
    public class SqlSettingsProvider : IConfigProvider {

        private readonly string _nameOrConnectionString;

        public SqlSettingsProvider(string nameOrConnectionString) {
            _nameOrConnectionString = nameOrConnectionString;
        }

        #region Implementation of IConfigProvider

        /// <summary>
        /// 获得配置文件
        /// </summary>
        /// <param name="key">配置文件的key</param>
        /// <param name="configType">配置文件的类型</param>
        /// <returns></returns>
        public IConfig Get(string key, Type configType) {
            using (var db = new DB(_nameOrConnectionString)) {
                ConfigInfo info = db.Set<ConfigInfo>().FirstOrDefault(x => x.Key == key);

                if (info == null) return null;

                object config = XMLHelper.XmlDeserialize(configType, info.Config, Encoding.UTF8);

                return config as IConfig;
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="key">配置文件的key</param>
        /// <param name="config">配置文件实例</param>
        public bool Save(string key, IConfig config) {
            try {
                using (var db = new DB(_nameOrConnectionString)) {
                    var info = db.Set<ConfigInfo>().FirstOrDefault(x => x.Key == key) ?? new ConfigInfo {
                                                                                                            Key = key
                                                                                                        };
                    info.Config = XMLHelper.XmlSerialize(config, Encoding.UTF8);

                    if(info.ID==0){
                        db.Set<ConfigInfo>().Add(info);
                    }
                    db.SaveChanges();

                    return true;
                }
            }
            catch {
                return false;
            }
        }

        #endregion
    }

    public class ConfigInfo {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Config { get; set; }
    }

    public class SettingMapping : EntityTypeConfiguration<ConfigInfo>
    {
        public SettingMapping()
        {
            HasKey(x => x.ID);

            ToTable(DB.TPref("conf"));
        }
    }
}