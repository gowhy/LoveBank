using System;

namespace LoveBank.Common
{
    public class IoC
    {
        private static IContainerAdapter _adapter;
        private static readonly object syncLock = new object();

        public static IContainerAdapter Current
        {
            get
            {
                if (_adapter == null) throw new ArgumentNullException("adapter", "Default ContainerAdapter don't be setted.");
                return _adapter;  
            }
            private set
            {
                lock (syncLock)
                {
                    _adapter = value;
                }
            }
        }

        public static void SetAdapter(IContainerAdapter adapter)
        {
            Current = adapter;
        }

        public static T Resolve<T>(string key)
        {

            return Current.Resolve<T>(key);
        }

        public static T Resolve<T>()
        {
            return Current.Resolve<T>();
        }
    }
}
