using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Core;
using LoveBank.Core.Domain;

namespace LoveBank.Services.ConfigModule
{
    public interface IConfigService : IServcie
    {
        IList<Config> LoadConfig();
        void SaveConfig(Config config);
    }
}
