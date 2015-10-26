namespace LoveBank.Common.Config {

    public class DefaultConfigManager {

        private readonly IConfigProvider _provider;

        public DefaultConfigManager(IConfigProvider provider) {

            _provider = provider;

        }
        
        public T LoadConfig<T>() where T : IConfig, new() {
            
            var key = typeof (T).FullName;

            return (T)_provider.Get(key, typeof(T));
        }

        public bool SaveConfig(IConfig config) {

            var key = config.GetType().FullName;

            return _provider.Save(key, config);
        }

    }
}