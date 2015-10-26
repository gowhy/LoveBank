using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Core;
using LoveBank.Core.Domain;

namespace LoveBank.Services.ConfigModule
{

    public class ConfigService : ServiceBase, IConfigService
    {
        public IList<Config> LoadConfig()
        {
            return DbProvider.D<Config>().ToList();
        }

        public void SaveConfig(Config config)
        {
            UpdateEntity(config);
        }
    }
}
